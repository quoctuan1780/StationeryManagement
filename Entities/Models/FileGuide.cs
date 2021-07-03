using System;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.TableNameConstant;

namespace Entities.Models
{
    [Table(TABLE_FILE_GUIDE)]
    public class FileGuide
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public string Type { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
