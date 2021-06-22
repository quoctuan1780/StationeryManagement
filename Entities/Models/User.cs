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

        public string Image { get; set; } 

        [StringLength(5, MinimumLength = 2)]
        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string StreetName { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public string WardCode { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual Ward Ward { get; set; }

        public virtual IList<Notification> Notifications { get; set; }

        public virtual IList<RatingDetail> RatingDetails { get; set; }

        public virtual IList<Order> Orders { get; set; }

        public virtual IList<CartItem> CartItems { get; set; }

        public  virtual IList<Bill> Bills { get; set; }

        public virtual IList<DeliveryAddress> DeliveryAddresses { get; set; }

        public virtual IList<Comment> Comments { get; set; }
        public  virtual IList<ReceiptRequest> ReceiptRequests { get; set; }
    }
}
