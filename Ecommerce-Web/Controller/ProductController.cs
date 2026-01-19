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
            // Take Product table from dbContext, join Category table and map to ProductVM
            var products = _dbContext.Products
                .Include(category => category.Category)
                .Select(productVM => new ProductVM
                {
                    Id = productVM.Id,
                    Name = productVM.Name,
                    Price = productVM.Price,
                    CategoryId = productVM.CategoryId,
                    CategoryName = productVM.Category != null ? productVM.Category.Name : string.Empty,
                    CreateAt = productVM.CreateAt,
                    IsActive = productVM.IsActive,
                    ImageUrl = productVM.ImageUrl
                }).ToList();

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

            //Reload dropdown of category
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

        [HttpGet]
        public IActionResult Edit(string? id)
        {
            if (id == null || id.Length == 0)
            {
                TempData["errorMessage"] = "Product not found";
                return NotFound();
            }

            var editProduct = _dbContext.Products.
                              FirstOrDefault(product => product.Id == id);

            if(editProduct == null)
            {
                TempData["errorMessage"] = "Product not found";
                return NotFound();
            }

            ProductVM productVM = new ProductVM
            {
                Id = editProduct.Id,
                Name = editProduct.Name,
                Price = editProduct.Price,
                CategoryId = editProduct.CategoryId,
                CreateAt = editProduct.CreateAt,
                IsActive = editProduct.IsActive,
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
        public IActionResult Edit(ProductVM productVM)
        {

            var updatedProduct = _dbContext.Products
                                 .FirstOrDefault(product => product.Id == productVM.Id);

            if (updatedProduct == null)
            {
                TempData["errorMessage"] = "Not found product";
                return NotFound();
            }

            // Update product
            if (ModelState.IsValid)
            {
                updatedProduct.Name = productVM.Name;
                updatedProduct.Price = productVM.Price;
                updatedProduct.CategoryId = productVM.CategoryId;
                updatedProduct.CreateAt = productVM.CreateAt;
                updatedProduct.IsActive = productVM.IsActive;

               _dbContext.SaveChanges();
               TempData["successMessage"] = "Product updated successfully";
               return RedirectToAction("Index");
            }

            // On error, reload categories and return view
            productVM.Categories = _dbContext.Categories.Select(cate => new SelectListItem
            {
                Value = cate.Id,
                Text = cate.Name
            }).ToList();

            return View(productVM);
        }
    }
}
