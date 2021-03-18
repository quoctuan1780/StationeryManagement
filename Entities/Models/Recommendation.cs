using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_RECOMMENDATION)]
    public class Recommendation
    { 
        public int RecommendtionId { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual IList<RecommendationDetail> RecommendationDetails { get; set; }
    }
}
