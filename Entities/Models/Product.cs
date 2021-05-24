using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_PRODUCT)]
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [RegularExpression(ValidationConstant.VALIDATE_NAME)]
        [StringLength(200, MinimumLength = 20)]
        public string ProductName { get; set; }

        [DataType(DataType.Currency)]
        [Range(1, 100)]
        public decimal Price { get; set; } = 0;

        public DateTime DateCreate { get; set; } = DateTime.Now;

        public int Total { get; set; }

        [Column(TypeName = "NTEXT")]
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual IList<SaleDetail> SaleDetails { get; set; }

        public virtual IList<RatingDetail> RatingDetails { get; set; }

        public virtual IList<RecommendationDetail> RecommendationDetails { get; set; }

        public virtual IList<ProductImage> ProductImages { get; set; }

        public virtual IList<ProductDetail> ProductDetails { get; set; }

        public virtual IList<Comment> Comments { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
