using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.Shipper.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Mật khẩu cũ không được để trống")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới không được để trống")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu nhập lại không được để trống")]
        [Compare("NewPassword", ErrorMessage = "Hai mật khẩu không giống nhau")]
        public string ConfirmPassword { get; set; }
    }
}
