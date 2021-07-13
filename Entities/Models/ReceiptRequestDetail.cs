using Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_RECEIPT_REQUEST_DETAIL)]
    public class ReceiptRequestDetail
    {
        public int ReceiptRequestId { get; set; }
        public virtual ReceiptRequest ReceiptRequest { get; set; }

        public int ProductDetailId { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }

        public int Quantity { get; set; } = 0;

        public string Status { get; set; }

        public decimal Price { get; set; }
    }
}