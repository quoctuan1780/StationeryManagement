using Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_SALE_PRODUCT)]
    public class SaleDetail
    {
        public int SaleDetailId { get; set; }
        public int SaleId { get; set; }
        public virtual Sale Sale { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public DateTime SaleStartDate { get; set; } = DateTime.Now;

        public DateTime SaleEndDate { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
