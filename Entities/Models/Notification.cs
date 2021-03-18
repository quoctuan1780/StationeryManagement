using Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_NOTIFICTION)]
    public class Notification
    {
        public int NotificationTypeId { get; set; }
        public virtual NotificationType NotificationType { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string Link { get; set; }
    }
}
