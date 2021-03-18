using Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_RECOMMENDATION_DETAIL)]
    public class RecommendationDetail
    {
        public int RecommendationId { get; set; }
        public virtual Recommendation Recommendation { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}