using Microsoft.AspNetCore.Mvc;
using Ecommerce_Web.Models;
using Ecommerce_Web.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Ecommerce_Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
