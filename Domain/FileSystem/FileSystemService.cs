using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Commands;
using NFS;

namespace Domain.FileSystem
{
    public class FileSystemService : IFileSystemService
    {
        private readonly IFileSystem _fileSystem;
        private readonly IGetFolderSizeCommand<GetFolderSizeResult, long, GetFolderSizeState> _getFolderSizeCommand;
        private readonly IUploadFileCommand<FileUploadResult, bool, UploadFileState> _uploadFileCommand;
        private readonly IRemoveCommand<RemoveResult, bool, RemoveState> _removeCommand;
        private readonly IDownloadFileCommand<FileDownloadResult, Stream, DownloadFileState> _downloadFileCommand;

        public FileSystemService(
            IFileSystem fileSystem,
            IGetFolderSizeCommand<GetFolderSizeResult, long, GetFolderSizeState> getFolderSizeCommand,
            IUploadFileCommand<FileUploadResult, bool, UploadFileState> uploadFileCommand,
            IRemoveCommand<RemoveResult, bool, RemoveState> removeCommand,
            IDownloadFileCommand<FileDownloadResult, Stream, DownloadFileState> downloadFileCommand
        )
        {
            _fileSystem = fileSystem;
            _getFolderSizeCommand = getFolderSizeCommand;
            _uploadFileCommand = uploadFileCommand;
            _removeCommand = removeCommand;
            _downloadFileCommand = downloadFileCommand;
        }

        private static long TimeToMilliseconds(DateTime time)
        {
            var milliseconds = new DateTimeOffset(time)
                .ToUnixTimeSeconds();

            return milliseconds;
        }

        private IEnumerable<TreeDTO> GetFileStructure(NPath node)
        {
            var files = _fileSystem
                .EnumerateFileEntries(node);

            // TODO: move to the factory

            return files.Select(file => new TreeDTO
                {
                    Id = file.FullName,
                    Date = TimeToMilliseconds(
                        _fileSystem
                            .GetLastWriteTime(new NPath(file.ToString()))),
                    Size = file.Length,
                    Type = NodeType.File,
                    Value = file.Name
                })
                .OrderBy(f => f.Value)
                .ToList();
        }

        private List<TreeDTO> GetFolderStructure(NPath node)
        {
            var directories = _fileSystem
                .EnumerateDirectories(node);

            var tree = new List<TreeDTO>();

            foreach (var dir in directories)
            {
                var data = GetFolderStructure(new NPath(dir.FullName));
                data.AddRange(GetFileStructure(new NPath(dir.FullName)));
                data = data
                    .OrderBy(f => f.Value)
                    .ToList();

                _getFolderSizeCommand
                    .Execute(
                        new GetFolderSizeState(new NPath(dir.FullName))
                    );

                var nodeDTO = new TreeDTO
                {
                    Id = dir.FullName,
                    Date = TimeToMilliseconds(
                        _fileSystem
                            .GetLastWriteTime(dir.Path)),
                    Size = _getFolderSizeCommand
                        .GetResult(),
                    Type = NodeType.Folder,
                    Value = new DirectoryInfo(dir.FullName).Name,
                    Data = data
                };

                tree.Add(nodeDTO);
            }

            return tree
                .OrderBy(n => n.Value)
                .ToList();
        }

        public async Task<List<TreeDTO>> GetStructureAsync()
        {
            return await Task.Run(() => GetFolderStructure(new NPath(System.IO.Directory.GetCurrentDirectory())));
        }

        public async Task<object> UploadFileAsync(UploadFileState state)
        {
            var result = await _uploadFileCommand.ExecuteAsync(state);

            return result;
        }

        public async Task<object> RemoveAsync(RemoveState state)
        {
            var result = await _removeCommand.ExecuteAsync(state);

            return result;
        }

        public async Task<Stream> DownloadFileAsync(DownloadFileState state)
        {
            await _downloadFileCommand.ExecuteAsync(state);

            return _downloadFileCommand.GetResult();
        }
    }
}