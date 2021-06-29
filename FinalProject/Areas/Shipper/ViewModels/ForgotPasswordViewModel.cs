using System.ComponentModel.DataAnnotations;
using static Common.ValidationConstant;

namespace FinalProject.Areas.Shipper.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = ERROR_PASSWORD_ENPTY)]
        public string Password { get; set; }

        [Required(ErrorMessage = ERROR_CONFIRMPASSWORD_ENPTY)]
        [Compare(COMPARE_PASSWORD, ErrorMessage = ERROR_COMPARE_PADDWORD)]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
