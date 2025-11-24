using System.ComponentModel;
using System.Diagnostics;
using AzureBlobProject.Models;
using AzureBlobProject.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IContainerService _containerService;

        public HomeController(IContainerService containerService)
        {
            _containerService = containerService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _containerService.GetContainersAndBlobsAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
