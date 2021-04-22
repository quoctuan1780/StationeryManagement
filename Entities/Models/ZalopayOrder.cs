using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class ZalopayOrder
    {
        [Key]
        public string Apptransid { get; set; }
        public string Zptransid { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public string Description { get; set; }
        public long Amount { get; set; }
        public long Timestamp { get; set; }
        public int Status { get; set; }
        public int Channel { get; set; }
    }
}
