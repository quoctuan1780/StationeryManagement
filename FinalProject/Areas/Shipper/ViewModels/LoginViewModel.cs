using static Common.ValidationConstant;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.Shipper.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = ERROR_EMAIL_EMPTY)]
        [DataType(DataType.EmailAddress, ErrorMessage = ERROR_EMAIL_TYPE)]
        public string Email { get; set; }
        [Required(ErrorMessage = ERROR_PASSWORD_ENPTY)]
        public string Password { get; set; }
    }
}
