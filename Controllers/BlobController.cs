using AzureBlobProject.Services.Interface;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult AddFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile formFile)
        {
            if (formFile == null || formFile.Length < 1) return View();

            var formFileName = Path.GetFileNameWithoutExtension(formFile.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(formFile.FileName);
            var result = await _blobService.UploadBlob(formFileName, formFile, "images");
            if (result)
            {
                return RedirectToAction("Index", "Container");
            }
            return View();
        }
    }
}
