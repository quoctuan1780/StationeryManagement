using Common;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = ValidationConstant.ERROR_EMAIL_EMPTY)]
        [DataType(DataType.EmailAddress, ErrorMessage = ValidationConstant.ERROR_EMAIL_TYPE)]
        public string Email { get; set; }
        [Required(ErrorMessage = ValidationConstant.ERROR_PASSWORD_ENPTY)]
        public string Password { get; set; }
    }
}
