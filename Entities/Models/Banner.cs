using System;

namespace Entities.Models
{
    public class Banner
    {
        public int BannerId { get; set; }
        public string Image { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Url { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
