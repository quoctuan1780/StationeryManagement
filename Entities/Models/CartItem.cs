using System;

namespace Entities.Models
{
    public class CartItem
    {
        public int ProductDetailId { get; set; }
        public ProductDetail ProductDetail { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateDate { get; set; }
        public bool isStocking { get; set; } = true;
    }
}
