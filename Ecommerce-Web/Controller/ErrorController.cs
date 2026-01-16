using Microsoft.AspNetCore.Mvc;
using Ecommerce_Web.Models;
using Ecommerce_Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult PageNotFound()
        {
            return View("PageNotFound");
        }

    }
}
