using System;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.TableNameConstant;

namespace Entities.Models
{
    [Table(TABLE_USER_CONNECTION_DETAIL)]
    public class UserConnectionDetail
    {
        public Guid UserConnectionDetailId { get; set; } = Guid.NewGuid();

        public Guid UserConnectionId { get; set; }
        public virtual UserConnection UserConnection { get; set; }
        public int ProductViewed { get; set; }

        public DateTime DateViewed { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;
    }
}
