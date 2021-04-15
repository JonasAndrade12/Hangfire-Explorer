namespace Hangfire_Explorer.Controllers
{
    using System.Diagnostics;
    using Hangfire_Explorer.Application.Interfaces;
    using Hangfire_Explorer.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IService service;

        public HomeController(ILogger<HomeController> logger, IService service)
        {
            _logger = logger;
            this.service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Exception()
        {
            this.service.GetExecption();
            return View();
        }

        public IActionResult Recurring()
        {
            this.service.CreateRecurring();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
