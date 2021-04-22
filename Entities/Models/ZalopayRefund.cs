using System.ComponentModel.DataAnnotations;
 
namespace Entities.Models
{
    public class ZalopayRefund
    {
        [Key]
        public string Mrefundid { get; set; }
        public string Zptransid { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public long Amount { get; set; }
    }
}
