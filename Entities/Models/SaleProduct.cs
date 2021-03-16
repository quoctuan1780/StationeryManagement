using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class SaleProduct
    {
        [Key]
        [Column(Order = 1)]
        public int SaleId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ProductId { get; set; }
        public DateTime SaleStartDate { get; set; } = DateTime.Now;
        public DateTime SaleEndDate { get; set; }
    }
}
