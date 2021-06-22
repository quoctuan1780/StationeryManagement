using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.Warehouse.ViewModels
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mật khẩu cũ không được để trống")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mật khẩu mới không được để trống")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mật khẩu nhập lại không được để trống")]
        [Compare("NewPassword", ErrorMessage = "Hai mật khẩu không giống nhau")]
        public string ConfirmPassword { get; set; }
    }
}
