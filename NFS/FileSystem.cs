using System;
using System.Collections.Generic;
using System.IO;

namespace NFS
{
    public abstract class FileSystem : IFileSystem
    {
        public void Dispose()
        {
            
        }

        public void CreateDirectory(NPath path)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExists(NPath path)
        {
            throw new NotImplementedException();
        }

        public void MoveDirectory(NPath srcPath, NPath destPath)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(NPath path, bool isRecursive)
        {
            throw new NotImplementedException();
        }

        public void ReplaceFile(NPath srcPath, NPath destPath, NPath destBackupPath, bool ignoreMetadataErrors)
        {
            throw new NotImplementedException();
        }

        public void CopyFile(NPath srcPath, NPath destPath, bool overwrite)
        {
            throw new NotImplementedException();
        }

        public void MoveFile(NPath srcPath, NPath destPath)
        {
            throw new NotImplementedException();
        }

        public bool FileExists(NPath path)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(NPath path)
        {
            throw new NotImplementedException();
        }

        public Stream OpenFile(NPath path, FileMode mode, FileAccess access, FileShare share = FileShare.None)
        {
            throw new NotImplementedException();
        }

        public long GetFileLength(NPath path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NPath> EnumeratePaths(NPath path, string searchPattern)
        {
            throw new NotImplementedException();
        }

        public FileAttributes GetAttributes(NPath path)
        {
            throw new NotImplementedException();
        }

        public void SetAttributes(NPath path, FileAttributes attributes)
        {
            throw new NotImplementedException();
        }

        public DateTime GetCreationTime(NPath path)
        {
            throw new NotImplementedException();
        }

        public void SetCreationTime(NPath path, DateTime time)
        {
            throw new NotImplementedException();
        }

        public void SetLastAccessTime(NPath path, DateTime time)
        {
            throw new NotImplementedException();
        }

        public void SetLastWriteTime(NPath path, DateTime time)
        {
            throw new NotImplementedException();
        }

        public DateTime GetLastAccessTime(NPath path)
        {
            throw new NotImplementedException();
        }

        public virtual DateTime GetLastWriteTimeImplementation(NPath path)
        {
            throw new NotImplementedException();
        }
        
        public DateTime GetLastWriteTime(NPath path)
        {
            return GetLastWriteTimeImplementation(path);
        }

        public string ConvertPathToInternal(NPath path)
        {
            throw new NotImplementedException();
        }

        public NPath ConvertPathFromInternal(string systemPath)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<File> EnumerateFileEntries(NPath path)
        {
            var files = System.IO.Directory.GetFiles(path.Path);

            foreach (var file in files)
            {
                // TODO: remove exposed logic to the ctor
                yield return new File
                {
                    Path = new NPath(file),
                    Length = new FileInfo(file).Length,
                    Name = Path.GetFileName(file)
                };
            }
        }

        public IEnumerable<Directory> EnumerateDirectories(NPath path)
        {
            var directories = System.IO.Directory.GetDirectories(path.Path);

            foreach (var directory in directories)
            {
                yield return new Directory
                {
                    Path = new NPath(directory),
                    Length = directory.Length,
                    Name = Path.GetDirectoryName(directory)
                };
            }
        }
    }
}