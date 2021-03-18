using Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_NOTIFICTION_TYPE)]
    public class NotificationType
    {
        public int NotificationTypeId { get; set; }

        [Required]
        public string NotificationTypeName { get; set; }

        public virtual IList<Notification> Notifications { get; set; }
    }
}
