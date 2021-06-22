using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.Shipper.ViewModels
{
    public class ChangePasswordViewModel
    {

        [DataType(DataType.Password)]
        [Required]
        [DisplayName("Mật khẩu hiện tại")]
        public string CurrentPass { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [DisplayName("Mật khẩu mới")]
        public string NewPass { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [DisplayName("Nhập lại mật khẩu")]
        [Compare("NewPass", ErrorMessage = "Mật khẩu không khớp!")]
        public string ConfirmedPass { get; set; }
        [Required(ErrorMessage = "Mật khẩu cũ không được để trống")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới không được để trống")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu nhập lại không được để trống")]
        [Compare("NewPassword", ErrorMessage = "Hai mật khẩu không giống nhau")]
        public string ConfirmPassword { get; set; }
    }
}
