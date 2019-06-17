using System;

namespace NFS
{
    public struct NPath : IEquatable<NPath>, IComparable<NPath>
    {
        public string Path { get; set; }

        public NPath(string path)
        {
            Path = path;
        }
        
        public bool Equals(NPath other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(NPath other)
        {
            throw new NotImplementedException();
        }
    }
}