using Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_CATEGORY)]
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [RegularExpression(ValidationConstant.VALIDATE_NAME)]
        [StringLength(30)]
        public string CategoryName { get; set; }

        public bool status { get; set; }

        public virtual IList<Product> Products { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}