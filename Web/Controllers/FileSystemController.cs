using System;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using Domain.FileSystem;
using Microsoft.AspNetCore.Mvc;
using Web.DTO;

namespace Web.Controllers
{
    public class FileSystemController : Controller
    {
        private readonly IFileSystemService _fileSystemService;

        public FileSystemController(
            IFileSystemService fileSystemService
        )
        {
            _fileSystemService = fileSystemService;
        }

        [HttpGet]
        [Route("api/data")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var tree = await _fileSystemService
                    .GetStructureAsync();

                return Json(tree);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        [Route("api/remove")]
        public async Task<IActionResult> Remove(
            [FromForm] RemoveRequest removeRequest)
        {
            try
            {
                if (removeRequest.source == null)
                {
                    return BadRequest("Remove target is not specified");
                }

                var targets = removeRequest.source.Split(",");

                foreach (var target in targets)
                {
                    if (Directory.Exists(target))
                    {
                        var folder = new DirectoryInfo(target);
                        await Task.Run(() => folder.Delete(true));
                    }
                    else if (System.IO.File.Exists(target))
                    {
                        var file = new FileInfo(target);
                        await Task.Run(() => file.Delete());
                    }
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        [Route("api/upload")]
        public async Task<IActionResult> UploadFile(
            [FromForm] UploadRequest uploadRequest)
        {
            try
            {
                if (uploadRequest.upload == null ||
                    uploadRequest.upload.Length == 0)
                {
                    return BadRequest("File not selected");
                }

                var path = Path.Combine(
                    uploadRequest.target,
                    uploadRequest.upload_fullpath);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await uploadRequest.upload.CopyToAsync(stream);
                }

                var result = new
                {
                    folder = uploadRequest.target,
                    value = uploadRequest.upload_fullpath,
                    id = uploadRequest.upload_fullpath,
                    type = "file",
                    status = "server"
                };
                
                return Json(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        [Route("api/download")]
        public async Task<IActionResult> Download(
            [FromForm] DownloadRequest downloadRequest)
        {
            try
            {
                if (downloadRequest == null)
                {
                    return BadRequest("Filename not present");
                }

                var path = downloadRequest.source;

                // This solution may result in OutOfMemory exception
                // for big files. Other solutions may be used instead
                // (e.g. Response.TransmitFile, manual chunking, setting OutputBuffer size to 0 etc.

                var memory = new MemoryStream();

                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }

                memory.Position = 0;

                return File(
                    memory,
                    "application/octet-stream",
                    Path.GetFileName(path));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}