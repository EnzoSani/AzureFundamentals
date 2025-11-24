using AzureBlobProject.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace AzureBlobProject.Controllers
{
    public class BlobController : Controller
    {
        private readonly IBlobService _blobService;
        public BlobController(IBlobService blobService)
        {
            _blobService = blobService;
        }
        [HttpGet]
        public async Task<IActionResult> Manage(string containerName)
        {
            var blobObj = await _blobService.GetAllBlobs(containerName);
            return View(blobObj);
        }

        [HttpGet]
        public IActionResult AddFile(string containerName)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(string containerName,IFormFile formFile)
        {
            if (formFile == null || formFile.Length < 1) return View();

            var formFileName = Path.GetFileNameWithoutExtension(formFile.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(formFile.FileName);
            var result = await _blobService.UploadBlob(formFileName, formFile, containerName);
            if (result)
            {
                return RedirectToAction("Index", "Container");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewFile(string name, string containerName)
        {
            return Redirect(await _blobService.GetBlob(name, containerName));
        }

        public async Task<IActionResult> DeleteFile(string name, string containerName)
        {
            await _blobService.DeleteBlob(name, containerName);
            return RedirectToAction("Index", "Home");
        }
    }
}
