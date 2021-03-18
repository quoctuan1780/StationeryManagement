using Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_BILL)]
    public class Bill
    {   
        public int BillId { get; set; }

        public DateTime CreateDate { get; set; }

        public decimal Total { get; set; }

        public decimal PurchaseTotal { get; set; }

        public decimal SaleTotal { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
