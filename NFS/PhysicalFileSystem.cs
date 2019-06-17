using System;

namespace NFS
{
    public class PhysicalFileSystem : FileSystem
    {
        public override DateTime GetLastWriteTimeImplementation(NPath path)
        {
            return System.IO.File.GetLastWriteTime(path.Path);
        }
    }
}