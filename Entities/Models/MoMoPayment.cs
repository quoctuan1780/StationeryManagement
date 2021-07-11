using Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_MOMO_PAYMENT)]
    public class MoMoPayment
    {
        public int MoMoPaymentId { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public string MoMoOrderId { get; set; }
        public string PayType { get; set; }
        public string Amount { get; set; }
        public string TransId { get; set; }
        public DateTime ResponseTime { get; set; }
    }
}
