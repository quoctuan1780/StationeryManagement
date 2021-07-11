using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_IMPORT_WAREHOUSE)]
    public class ImportWarehouse
    {
        public int ImportWarehouseId { get; set; }

        public DateTime CreateDate { get; set; }

        //public DateTime ImportDate { get; set; }

        public decimal Total { get; set; }

        public string UserId { get; set; }

        public string Status { get; set; }

        public virtual IList<ImportWarehouseDetail> ImportWarehouseDetails { get; set; }

        public int ReceiptRequestId { get; set; }
        public virtual ReceiptRequest ReceiptRequest { get; set; }
    }
}
