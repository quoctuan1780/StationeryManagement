using Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_IMPORT_WAREHOUSE_DETAIL)]
    public class ImportWarehouseDetail
    {
        public int ImportWarehouseId { get; set; }

        public int ProductDetailId { get; set; }

        public int Quantity { get; set; } = 0;

        public int ActualQuantity { get; set; } = 0;

        public decimal ImportPrice { get; set; }

        public string Status { get; set; }

        public virtual ImportWarehouse ImportWarehouse { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
    }
}