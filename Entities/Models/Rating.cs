using Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_RATING)]
    public class Rating
    {
        public int RatingId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string RatingTypeName { get; set; }
        public byte RatingNumber { get; set; }
        public virtual IList<RatingDetail> RatingDetails { get; set; }
    }
}
