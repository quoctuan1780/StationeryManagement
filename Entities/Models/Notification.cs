using Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.PortableExecutable;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_NOTIFICTION)]
    public class Notification
    {
        public int NotificationId { get; set; }
        public int NotificationTypeId { get; set; }
        public virtual NotificationType NotificationType { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int RecordId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Status { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Link { get; set; }
        public bool IsDeleted { get; set; } = false;
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string RoleSeen { get; set; }
    }
}
