using Ecommerce_Web.Data;
using Ecommerce_Web.Models;
using Ecommerce_Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Ecommerce_Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ApplicationDbContext dbContext, ILogger<ProductController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public IActionResult Index()
        {
            List<Product> products = _dbContext.Products.ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //Bind categories to dropdown
            var productVM = new ProductVM()
            {
                Categories = _dbContext.Categories.Select(c => new SelectListItem
                {
                    Value = c.Id,
                    Text = c.Name
                }).ToList()
            };
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = productVM.Name,
                    Price = productVM.Price,
                    CategoryId = productVM.CategoryId,
                    CreateAt = productVM.CreateAt,
                    IsActive = productVM.IsActive

                };

                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
                TempData["successMessage"] = "Add product successfully";
            }

            //Reload dropdown when error happens
            else
            {
                productVM.Categories = _dbContext.Categories.Select(c => new SelectListItem
                {
                    Value = c.Id,
                    Text = c.Name
                }).ToList();
                TempData["errorMessage"] = "Add Product invalid";
                return View(productVM);
            }

            return RedirectToAction("Index");
        }
    }
}
