namespace NFS
{
    public class File : IFileSystemEntry
    {
        public string Name { get; set; }
        public long Length { get; set; }

        public string FullName => Path.Path;
        public NPath Path { get; set; }
    }
}