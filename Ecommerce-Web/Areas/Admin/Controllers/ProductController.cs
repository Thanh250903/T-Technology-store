using Ecommerce_Web.Data;
using Ecommerce_Web.Models;
using Ecommerce_Web.Models.Catalog;
using Ecommerce_Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;

namespace Ecommerce_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ProductController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext dbContext, ILogger<ProductController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
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
                Categories = _dbContext.Categories
                                       .Select(c => new SelectListItem
                {
                    Value = c.Id,
                    Text = c.Name
                }).ToList()
            };
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM productVM, IFormFile? formFile)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (formFile != null && formFile.Length > 0)
                {
                    // upload image to wwwroot/img/products
                    string uploadsFolder = Path.Combine(wwwRootPath, @"img\products");
                    string fileName = Path.GetFileName(formFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    try
                    {
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            formFile.CopyTo(fileStream);
                        }
                        productVM.ImageUrl = @"\img\products\" + fileName;

                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Image upload error: " + ex.Message);
                        return View(productVM);
                    }
                }

                var product = new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = productVM.Name,
                    Price = productVM.Price,
                    CategoryId = productVM.CategoryId,
                    CreateAt = productVM.CreateAt,
                    IsActive = productVM.IsActive,
                    ImageUrl = productVM.ImageUrl
                };

                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
                TempData["successMessage"] = "Add product successfully";
            }

            //Reload dropdown of category
            else
            {
                productVM.Categories = _dbContext.Categories
                                       .Select(c => new SelectListItem
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
                ImageUrl = editProduct.ImageUrl,
                Categories = _dbContext.Categories
                             .Select(c => new SelectListItem
                {
                    Value = c.Id,
                    Text = c.Name
                }).ToList()
            };

            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM productVM, IFormFile? formFile)
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

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (formFile != null && formFile.Length > 0)
                {
                    // upload image to wwwroot/img/products
                    string uploadsFolder = Path.Combine(wwwRootPath, @"img\products");
                    string fileName = Path.GetFileName(formFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using(var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(fileStream);
                    }
                    updatedProduct.ImageUrl = @"\img\products\" + fileName;
                }
                   
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
            productVM.Categories = _dbContext.Categories
                                   .Select(cate => new SelectListItem
            {
                Value = cate.Id,
                Text = cate.Name
            }).ToList();

            return View(productVM);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            if(id == null || id.Length == 0)
            {
                TempData["errorMessage"] = "Product not found";
                return NotFound();
            }

            var deletedProduct = await _dbContext.Products
                                 .Include(category => category.Category)
                                 .FirstOrDefaultAsync(product => product.Id == id);

            if (deletedProduct == null)
            {
                TempData["errorMessage"] = "Product not found";
                return NotFound();
            }

            ProductVM productVM = new ProductVM
            {
                Id = deletedProduct.Id,
                Name = deletedProduct.Name,
                Price = deletedProduct.Price,
                CategoryId = deletedProduct.CategoryId,
                CategoryName = deletedProduct.Category.Name != null ? deletedProduct.Category.Name : string.Empty,
                CreateAt = deletedProduct.CreateAt,
                IsActive = deletedProduct.IsActive,
                ImageUrl = deletedProduct.ImageUrl
            };

            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(ProductVM productVM)
        {
            
            if (ModelState.IsValid)
            {
                var productDeleted = _dbContext.Products
                                     .FirstOrDefault(product => product.Id == productVM.Id);

                if (productDeleted == null)
                {
                    TempData["errorMessage"] = "Product not found";
                    return NotFound();
                }

                _dbContext.Products.Remove(productDeleted);
                _dbContext.SaveChanges();
                TempData["successMessage"] = "Product deleted successfully";
            }

            return RedirectToAction("Index");
        }
    }
}
