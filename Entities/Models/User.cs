using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(200, MinimumLength = 10)]
        public string FullName { get; set; } 

        [Required]
        [StringLength(300, MinimumLength = 10)]
        public string Address { get; set; }

        public string Image { get; set; } 

        [StringLength(5, MinimumLength = 2)]
        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public virtual IList<Notification> Notifications { get; set; }

        public virtual IList<RatingDetail> RatingDetails { get; set; }

        public virtual IList<Order> Orders { get; set; }
    }
}
