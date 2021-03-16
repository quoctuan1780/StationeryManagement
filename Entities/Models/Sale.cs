using Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Sale
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(ValidationConstant.VALIDATE_NAME, ErrorMessage = ValidationConstant.ERROR_FORMAT)]
        [StringLength(100, MinimumLength = 10)]
        public string SaleName { get; set; }

        [Range(0, 100, ErrorMessage = ValidationConstant.ERROR_DISCOUNT_RANGE)]
        public decimal Discount { get; set; } = 0;
        public virtual IList<SaleProduct> SaleProducts { get; set; }
    }
}
