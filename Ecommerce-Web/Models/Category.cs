using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Web.Models
{
    public class Category
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Product>? Products { get; set; }

    }
}
