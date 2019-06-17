using System.Threading.Tasks;
using Domain.FileSystem;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class FileSystemController : Controller
    {
        private readonly IFileSystemService _fileSystemService;

        public FileSystemController(IFileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService;
        }
        
        [HttpGet]
        [Route("api/data")]
        public async Task<JsonResult> Index()
        {
            var tree = await _fileSystemService.GetStructureAsync();

            return Json(tree);
        }
    }
}