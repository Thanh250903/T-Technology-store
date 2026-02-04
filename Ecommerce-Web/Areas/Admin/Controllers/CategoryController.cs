using Ecommerce_Web.Data;
using Ecommerce_Web.Models.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryController(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }
        // GET: Category
        public IActionResult Index()
        {
            List<Category> categories = _dbContext.Categories.ToList();
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            // ensure Id exists before any DB operation
            if (string.IsNullOrWhiteSpace(category.Id))
            {
                category.Id = Guid.NewGuid().ToString();
            }
            // Delete old validation of Id
            ModelState.Remove(nameof(Category.Id));

            if (ModelState.IsValid)
            {
                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();
                TempData["successMessage"] = "Category created successfully!";
                return RedirectToAction("Index");
            }

            TempData["errorMessage"] = "Invalid data. Please try again.";
            return View(category);
        }

        public IActionResult Edit(string? id)
        {
            if(id == null || id.Length == 0)
            {
                TempData["errorMessage"] = "Category not found.";
                return NotFound();
            }

            var editCategory = _dbContext.Categories.
                            FirstOrDefault(category => category.Id == id);

            if(editCategory == null)
            {
                TempData["errorMessage"] = "Category not found.";
                return NotFound();
            }
            return View(editCategory);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();
            TempData["successMessage"] = "Category updated successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(string? id)
        {
            if(id == null || id.Length == 0)
            {
                TempData["errorMessage"] = "Category not found.";
                return NotFound();
            }

            var deletedCategory = _dbContext.Categories.
                                  FirstOrDefault(category => category.Id == id);

            if(deletedCategory == null)
            {
                TempData["errorMessage"] = "Category not found.";
                return NotFound();
            }
            return View(deletedCategory);
        }

        [HttpPost]
        public IActionResult Delete(Category category)
        {
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
            TempData["successMessage"] = "Category deleted successfully!";
            return RedirectToAction("Index");
        }

    }
}
