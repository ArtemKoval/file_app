using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.FileSystem
{
    public interface IFileSystemService
    {
        Task<List<TreeDTO>> GetStructureAsync();
    }
}