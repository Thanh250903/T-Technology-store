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
    }
}
