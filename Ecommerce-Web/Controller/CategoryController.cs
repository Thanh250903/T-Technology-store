using Microsoft.AspNetCore.Mvc;
using Ecommerce_Web.Models;
using Ecommerce_Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Web.Controllers
{
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
        public IActionResult Create(Category category)
        {
            category.Id = Guid.NewGuid().ToString();
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(string? id)
        {
            if(id == null || id.Length == 0)
            {
                return NotFound();
            }

            var editCategory = _dbContext.Categories.
                            FirstOrDefault(category => category.Id == id);

            if(editCategory == null)
            {
                return NotFound();
            }
            return View(editCategory);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(string? id)
        {
            if(id == null || id.Length == 0)
            {
                return NotFound();
            }

            var deletedCategory = _dbContext.Categories.
                                  FirstOrDefault(category => category.Id == id);

            if(deletedCategory == null)
            {
                return NotFound();
            }
            return View(deletedCategory);
        }

        [HttpPost]
        public IActionResult Delete(Category category)
        {
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
