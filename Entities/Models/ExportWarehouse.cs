using Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_EXPORT_WAREHOUSE)]
    public class ExportWarehouse
    {
        public int ExportWarehouseId { get; set; }

        public DateTime CreateDate { get; set; }

        public decimal Total { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
