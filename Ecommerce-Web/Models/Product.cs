using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_Web.Models
{
    public class Product
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        //public int Quantity { get; set; }
        public decimal Price { get; set; }
        [Required]
        [MaxLength(450)]
        [ForeignKey("Category")]
        public string CategoryId { get; set; } // FK

        [ValidateNever]
        public Category Category { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsActive { get; set; }

        public Product()
        {
            CreateAt = DateTime.Now;
            IsActive = true;
        }

    }
}
