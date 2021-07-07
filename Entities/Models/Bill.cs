using Common;
using System;
using System.Collections.Generic;
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

        public decimal SaleTotal { get; set; }

        [StringLength(10)]
        public string PaymentMethod { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual IList<BillDetail> BillDetails { get; set; }
    }
}
