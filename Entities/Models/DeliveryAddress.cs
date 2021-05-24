using Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_DELIVERY_ADDRESS)]
    public class DeliveryAddress
    {
        public int DeliveryAddressId { get; set; }
        public string WardCode { get; set; }
        public virtual Ward Ward { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string StreetName { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
