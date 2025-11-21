using AzureBlobProject.Models;
using AzureBlobProject.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobProject.Controllers
{
    public class ContainerController : Controller
    {
        private readonly IContainerService _containerService;
        public ContainerController(IContainerService containerService)
        {
            _containerService = containerService;
        }
        public async Task<IActionResult> Index()
        {
            var containers = await _containerService.GetAllContainerAsync();
            return View(containers);
        }

        public async Task<IActionResult> Delete(string containerName)
        {
            await _containerService.DeleteContainerAsync(containerName);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            return View(new Container());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Container container)
        {
            await _containerService.CreateContainerAsync(container.Name);
            return RedirectToAction(nameof(Index));
        }
    }
}
