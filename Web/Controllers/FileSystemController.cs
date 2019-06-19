using System;
using System.IO;
using System.Threading.Tasks;
using Domain.Commands;
using Domain.FileSystem;
using Microsoft.AspNetCore.Mvc;
using NFS;
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

                var result = await _fileSystemService
                    .RemoveAsync(
                        new RemoveState(new NPath(removeRequest.source)));

                if (result == null)
                {
                    throw new SystemException("Unable to process remove");
                }

                return Json(result);
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

                var result = await _fileSystemService
                    .UploadFileAsync(
                        new UploadFileState(
                            new NPath(uploadRequest.target),
                            new NPath(uploadRequest.upload_fullpath),
                            uploadRequest.upload.OpenReadStream()
                        ));

                if (result == null)
                {
                    throw new SystemException("Unable to process file upload");
                }

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

                var result = await _fileSystemService
                    .DownloadFileAsync(
                        new DownloadFileState(new NPath(downloadRequest.source)));

                if (result == null)
                {
                    throw new SystemException("Unable to process file download");
                }

                return File(
                    result,
                    "application/octet-stream",
                    Path.GetFileName(downloadRequest.source));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}