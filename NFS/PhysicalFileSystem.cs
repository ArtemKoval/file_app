using System;
using System.Collections.Generic;
using System.IO;

namespace NFS
{
    public class PhysicalFileSystem : FileSystem
    {
        protected override DateTime GetLastWriteTimeImplementation(NPath path)
        {
            return System.IO.File.GetLastWriteTime(path.Path);
        }

        protected override Stream OpenFileImplementation(
            NPath path,
            FileMode mode,
            FileAccess access,
            FileShare share = FileShare.None)
        {
            return new FileStream(path.Path, mode, access, share);
        }

        protected override bool FileExistsImplementation(NPath path)
        {
            return System.IO.File.Exists(path.Path);
        }

        protected override bool DirectoryExistsImplementation(NPath path)
        {
            return System.IO.Directory.Exists(path.Path);
        }

        protected override IEnumerable<File> EnumerateFileEntriesImplementation(NPath path)
        {
            var files = System.IO.Directory.GetFiles(path.Path);

            foreach (var file in files)
            {
                yield return new File(
                    Path.GetFileName(file),
                    new FileInfo(file).Length,
                    new NPath(file));
            }
        }

        protected override IEnumerable<Directory> EnumerateDirectoriesImplementation(NPath path)
        {
            var directories = System.IO.Directory.GetDirectories(path.Path);

            foreach (var directory in directories)
            {
                yield return new Directory(
                    Path.GetDirectoryName(directory),
                    0,
                    new NPath(directory)
                );
            }
        }

        protected override void DeleteDirectoryImplementation(NPath path, bool isRecursive)
        {
            var folder = new DirectoryInfo(path.Path);
            folder.Delete(isRecursive);
        }

        protected override void DeleteFileImplementation(NPath path)
        {
            var file = new FileInfo(path.Path);
            file.Delete();
        }

        protected override NPath PathCombineImplementation(NPath path1, NPath path2)
        {
            var path = Path.Combine(
                path1.Path,
                path2.Path);

            return new NPath(path);
        }
    }
}