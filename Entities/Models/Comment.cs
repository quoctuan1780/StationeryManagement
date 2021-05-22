using System;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.TableNameConstant;

namespace Entities.Models
{
    [Table(TABLE_COMMENT)]
    public class Comment
    {
        public int CommentId { get; set; }
        public int? ReplyCommentId { get; set; }
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public string TagName { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
