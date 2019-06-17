using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NFS;

namespace Domain.FileSystem
{
    public class FileSystemService : IFileSystemService
    {
        private readonly IFileSystem _fileSystem;

        public FileSystemService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        private static long TimeToMilliseconds(DateTime time)
        {
            var milliseconds = new DateTimeOffset(time).ToUnixTimeSeconds();

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
                    Date = TimeToMilliseconds(_fileSystem.GetLastWriteTime(new NPath(file.ToString()))),
                    Size = file.Length,
                    Type = "file",
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

                var nodeDTO = new TreeDTO
                {
                    Id = dir.FullName,
                    Date = TimeToMilliseconds(_fileSystem.GetLastWriteTime(dir.Path)),
                    Size = 0,
                    Type = "folder",
                    Value = dir.FullName,
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
    }
}