using Ecommerce_Web.Models.Catalog;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_Web.Models.Cart
{
    public class OrderDetail
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid OrderId { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải ít nhất là 1")]
        public int Quantity { get; set; }
        // Giá của 1 sản phẩm tại thời điểm mua (snapshot) – VNĐ
        // Vì Product có thể đổi giá sau này nhưng đơn hàng cũ phải giữ nguyên
        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitPriceVnd { get; set; }

        // Thành tiền của dòng = UnitPrice * Quantity – VNĐ
        // Lưu sẵn để query nhanh và tránh tính lại nhiều lần
        [Required]
        [Range(0, double.MaxValue)]
        public decimal LineTotalVnd { get; set; }
        [ForeignKey("OrderId")]
        public Orders? Order { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

    }
}
