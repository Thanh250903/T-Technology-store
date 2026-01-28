using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ecommerce_Web.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }
        public int? Age { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public string? ProfileImageUrl { get; set; }

    }
}
