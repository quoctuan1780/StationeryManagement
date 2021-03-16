using Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class User : IdentityUser
    {
        [Required, StringLength(200, MinimumLength = 10, ErrorMessage = "out of range")]
        public string FullName { get; set; } = Constant.EMPTY;

        [Required, StringLength(300, MinimumLength = 10, ErrorMessage = "out of range")]
        public string Address { get; set; } = Constant.EMPTY;

        public string Image { get; set; } = Constant.EMPTY;
    }
}
