using Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_IMPORT_WAREHOUSE_DETAIL)]
    public class ImportWarehouseDetail
    {
        public int ImportWarehouseId { get; set; }

        public int ProviderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; } = 0;

        public decimal ImportPrice { get; set; }

        public virtual ImportWarehouse ImportWarehouse { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual Product Product { get; set; }
    }
}