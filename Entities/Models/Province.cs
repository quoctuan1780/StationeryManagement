using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Province
    {
        public int ProvinceID { get; set; }

        [StringLength(40)]
        public string ProvinceName { get; set; }
        public int Code { get; set; }
        public virtual IList<District> Districts { get; set; }
    }
}
