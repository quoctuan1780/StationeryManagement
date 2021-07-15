using Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_RATING_DETAIL)]
    public class RatingDetail
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int RatingId { get; set; }
        public virtual Rating Rating { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public DateTime RatingDate { get; set; }

        //public string Image { get; set; }

        public string Content { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
