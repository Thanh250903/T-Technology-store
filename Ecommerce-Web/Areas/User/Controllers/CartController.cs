using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce_Web.Areas.User.Controllers
{
    public class CartController : Controller
    {
        [Area("User")]
        [Authorize]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = new List<object>();

            return View(cartItems);
        }
    }
}
