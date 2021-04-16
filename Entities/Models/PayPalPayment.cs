using Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_PAYPAL_PAYMENT)]
    public class PayPalPayment
    {
        public int PayPalPaymentId { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public string Token { get; set; }
        public string  PayerId { get; set; }
        public string LinkDetail { get; set; }
    }
}
