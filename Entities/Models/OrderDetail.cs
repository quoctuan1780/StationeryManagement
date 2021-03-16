using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class OrderDetail
    {
        [Key, Column(Order = 1)]
        public int OrderId { get; set; }
        [Key, Column(Order = 2)]
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 0;
        public decimal SalePrice { get; set; }
        public decimal Price { get; set; }
        public virtual Order Order { get; set; }
    }
}