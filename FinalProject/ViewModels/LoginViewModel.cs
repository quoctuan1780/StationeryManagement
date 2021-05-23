using static Common.ValidationConstant;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = ERROR_EMAIL_EMPTY)]
        [DataType(DataType.EmailAddress, ErrorMessage = ERROR_EMAIL_TYPE)]
        public string Email { get; set; }
        [Required(ErrorMessage = ERROR_PASSWORD_ENPTY)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
