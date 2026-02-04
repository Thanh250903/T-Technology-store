using Ecommerce_Web.Models.User;
using Ecommerce_Web.Models.Catalog;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_Web.Models.Cart
{
    public class Cart
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string UserId { get; set; } = default!;
        [Required]
        public string ProductId { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng vật phẩm phải ít nhất là 1")]
        [Required(ErrorMessage = "Số lượng vật phẩm là bắt buộc")]
        public int Count { get; set; } = 1;
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

    }
}
