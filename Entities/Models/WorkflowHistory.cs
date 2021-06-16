using System;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.TableNameConstant;

namespace Entities.Models
{
    [Table(name: TABLE_WORKFLOW_STATUS)]
    public class WorkflowHistory
    {
        public int WorkflowHistoryId { get; set; }
        public string RecordId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public string CurrentStatus { get; set; }
        public string NextStatus { get; set; }
        public string UserEmail { get; set; }
        public string FullName { get; set; }
        public string  UserRole { get; set; }
        public string Type { get; set; } = "Đặt hàng";
        public bool IsDeleted { get; set; } = false;
    }
}
