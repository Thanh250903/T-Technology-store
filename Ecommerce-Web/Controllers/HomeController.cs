using Microsoft.AspNetCore.Mvc;
using Ecommerce_Web.Models;
using Ecommerce_Web.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Ecommerce_Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController>? _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var productList = await _context.Products.
                                    Where(activeP => activeP.IsActive).ToListAsync();

            return View(productList);
        }

        public async Task<IActionResult> Details(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var productDetails = await _context.Products
                                       .FirstOrDefaultAsync(p => p.Id == id && p.IsActive == true);

            if(productDetails == null)
            {
                return NotFound();
            }

            return View(productDetails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
