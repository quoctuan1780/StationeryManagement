using Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_PROVIDER)]
    public class Provider
    {
        public int ProviderId { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string ProviderName { get; set; }

        public virtual IList<ImportWarehouseDetail> ImportWarehouseDetails { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
