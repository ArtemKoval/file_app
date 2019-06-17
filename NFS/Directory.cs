namespace NFS
{
    public class Directory : IFileSystemEntry
    {

        public string Name { get; set; }

        public string FullName => Path.Path;

        public long Length { get; set; }
        public NPath Path { get; set; }
    }
}