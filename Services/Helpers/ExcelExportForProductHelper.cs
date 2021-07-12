using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Helpers
{
    public class ExcelExportForProductHelper
    {
        [Column("Tên sản phẩm")]
        public string ProductName { get; set; }
        [Column("Phân Loại sản phẩm")]
        public string Color { get; set; }
        [Column("Số lượng đã bán")]
        public int QuantitySold { get; set; }
        [Column("Số lượng còn lại")]
        public int QuantityRemaining { get; set; }
        [Column("Doanh thu")]
        public decimal Revenue { get; set; }
    }
}
