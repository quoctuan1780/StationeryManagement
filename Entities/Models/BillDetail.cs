using Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_BILL_DETAIL)]
    public class BillDetail
    {
        public int BillId { get; set; }
        public virtual Bill Bill { get; set; }

        public int ProductDetailId { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }

        public int Quantity { get; set; } = 0;

        public decimal SalePrice { get; set; }

        public decimal Price { get; set; }
        public string Status { get; set; }
    }
}
