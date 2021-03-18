using Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_ORDER_DETAIL)]
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int Quantity { get; set; } = 0;

        public decimal SalePrice { get; set; }

        public decimal Price { get; set; }
    }
}