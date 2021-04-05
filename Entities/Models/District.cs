using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class District
    {
        public int DistrictID { get; set; }
        [StringLength(80)]
        public string DistrictName { get; set; }
        public string Code { get; set; }
        public int ProvinceID { get; set; }
        public virtual Province Province { get; set; }
        public virtual IList<Ward> Wards { get; set; }
    }
}
