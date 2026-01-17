using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Web.Models.ViewModels
{
    public class ProductVM
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; } //FK
        public string? ImageUrl { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsActive { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}
