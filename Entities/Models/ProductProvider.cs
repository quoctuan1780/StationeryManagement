using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class ProductProvider
    {
        [Key, Column(Order = 1)]
        public int ProductId { get; set; }
        [Key, Column(Order = 2)]
        public int ProviderId { get; set; }
        public virtual Product Product { get; set; }
        public virtual Provider Provider { get; set; }
    }
}
