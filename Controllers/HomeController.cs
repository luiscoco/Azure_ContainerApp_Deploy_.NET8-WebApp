using AzureContainerAppWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AzureContainerAppWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public class Todo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool Done { get; set; }
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Todos = new List<Todo>()

            {
                new Todo 
                { 
                    Id = 1,
                    Name = "Test1",
                },
                new Todo
                {
                    Id = 1,
                    Name = "Test2",
                }
            };
            return View();
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
