using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Helpers
{
    public class ExcelExportHelper
    {
        [Column("Khách hàng")]
        public string FullName { get; set; }
        [Column("Tổng đơn hàng")]
        public int QuantityOfOrder { get; set; }
        [Column("Tổng tiền")]
        public decimal TotalOfOrder { get; set; }
        [Column("Tổng đơn đã thanh toán")]
        public int QuantityOfOrderPaid { get; set; }
        [Column("Tổng tiền đã thanh toán")]
        public decimal TotalOfOrderPaid { get; set; }
        [Column("Tổng số đơn hủy")]
        public int QuantityOfOrderRejected { get; set; }
        [Column("Tổng tiền hủy")]
        public decimal TotalOfOrderRejected { get; set; }
        [Column("Doanh thu")]
        public decimal Revenue { get; set; }
    }
}
