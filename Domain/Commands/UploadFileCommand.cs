using System;
using System.IO;
using System.Threading.Tasks;
using Domain.FileSystem;
using NFS;

namespace Domain.Commands
{
    public class UploadFileCommand
        : IUploadFileCommand<FileUploadResult, bool, UploadFileState>
    {
        private readonly IFileSystem _fileSystem;

        public UploadFileCommand(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public async Task<FileUploadResult> ExecuteAsync(UploadFileState state)
        {
            return await Task.Run(() => Execute(state));
        }

        public FileUploadResult Execute(UploadFileState state)
        {
            try
            {
                var path = _fileSystem.PathCombine(
                    state.Target,
                    state.FileName);

                using (var stream = _fileSystem.OpenFile(
                    path,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.Write))
                {
                    state.Stream.CopyTo(stream);
                }

                Result = new FileUploadResult(true, new
                {
                    folder = state.Target.Path,
                    value = state.FileName.Path,
                    id = state.FileName.Path,
                    type = NodeType.File,
                    status = "server"
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                Result = new FileUploadResult(false, null);
            }

            return Result;
        }

        public bool GetResult()
        {
            throw new NotImplementedException();
        }

        public FileUploadResult Result { get; private set; }
    }
}