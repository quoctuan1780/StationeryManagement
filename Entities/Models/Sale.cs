using Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_SALE)]
    public class Sale
    {
        public int SaleId { get; set; }

        [Required]
        [RegularExpression(ValidationConstant.VALIDATE_NAME, ErrorMessage = ValidationConstant.ERROR_FORMAT)]
        [StringLength(100, MinimumLength = 10)]
        public string SaleName { get; set; }

        [Range(0, 100, ErrorMessage = ValidationConstant.ERROR_DISCOUNT_RANGE)]
        public decimal Discount { get; set; } = 0;
        public virtual IList<SaleDetail> SaleDetails { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
