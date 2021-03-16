using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        // dia chi giao hang
        [Required, StringLength(200, MinimumLength = 10)]
        public string Address { get; set; }
        public IList<OrderDetail> OrderDetails { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}