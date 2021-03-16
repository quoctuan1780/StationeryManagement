using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Product
    {
        public int Id { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$"), Required, StringLength(200, MinimumLength = 20)]
        public string ProductName { get; set; }

        [DataType(DataType.Currency), Range(1, 100)]
        public decimal Price { get; set; } = 0;
        public DateTime DateCreate { get; set; } = DateTime.Now;
        [StringLength(255)]
        public string Image { get; set; } = "";
        [Required]
        public string Description { get; set; }

        public virtual Category Category { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual IList<OrderDetail> OrderDetails { get; set; }
        public virtual IList<ProductProvider> ProductProviders { get; set; }
        public virtual IList<SaleProduct> SaleProducts { get; set; }
    }
}
