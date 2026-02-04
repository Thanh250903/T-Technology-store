using Ecommerce_Web.Models.Enum;
using Ecommerce_Web.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce_Web.Models.Cart
{
    public class Orders
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string UserId { get; set; } = default!;
        [Required]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public decimal TotalAmount { get; set; }
        // Tên người nhận tại thời điểm đặt hàng
        [Required]
        public string? ReceiverName { get; set; } = default!;

        // Số điện thoại người nhận (snapshot, không phụ thuộc User)
        [Required]
        public string? Phone { get; set; } = default!;

        // Địa chỉ giao hàng (snapshot, đề phòng user đổi địa chỉ)
        [Required]
        public string? ShippingAddress { get; set; } = default!;

        // Ghi chú thêm của khách 
        public string? Note { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

    }
}
