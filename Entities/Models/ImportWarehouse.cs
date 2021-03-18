using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_IMPORT_WAREHOUSE)]
    public class ImportWarehouse
    {
        public int ImportWarehouseId { get; set; }

        public DateTime CreateDate { get; set; }

        public decimal Total { get; set; }

        public virtual IList<ImportWarehouseDetail> ImportWarehouseDetails { get; set; }
    }
}
