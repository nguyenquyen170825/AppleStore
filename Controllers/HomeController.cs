using System.Diagnostics;
using DUANCUAHANGAPPLE.Models;
using Microsoft.AspNetCore.Mvc;

namespace DUANCUAHANGAPPLE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        //----------------USER-------------------
        public IActionResult Index()
        {
            return View("User/Index");
        }

        public IActionResult Privacy()
        {
            return View("User/Privacy");
        }
        public IActionResult Login(){
            return View("User/Account/Login");
        }
        public IActionResult Register(){
            return View("User/Register");
        }
        //--------------ADMIN--------------------



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
