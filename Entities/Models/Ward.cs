using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Ward
    {
        [StringLength(10)]
        public string WardCode { get; set; }
        public int DistrictId { get; set; }
        public virtual District District { get; set; }

        [StringLength(80)]
        public string WardName { get; set; }

        public virtual User User { get; set; }
    }
}
