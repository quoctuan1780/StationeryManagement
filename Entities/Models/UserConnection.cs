using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.TableNameConstant;

namespace Entities.Models
{
    [Table(TABLE_USER_CONNECTION)]
    public class UserConnection
    {
        public Guid UserConnectionId { get; set; } = Guid.NewGuid();

        public string ClientConnectionId { get; set; }

        public DateTime FirstAccessedDate { get; set; } = DateTime.Now;

        public DateTime LastAccessedDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        public virtual IList<UserConnectionDetail> UserConnectionDetails { get; set; }
    }
}
