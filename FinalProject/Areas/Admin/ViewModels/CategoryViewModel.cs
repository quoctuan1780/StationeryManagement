using Common;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.Admin.ViewModels
{
    public class CategoryViewModel
    {
        [Required(ErrorMessage = ValidationConstant.ERROR_CATEGORY_NAME_EMPTY)]
        [StringLength(30, ErrorMessage = ValidationConstant.ERROR_CATEGORY_NAME_RANGE)]
        [Display(Name = DisplayNameConstant.DISPLAY_CATEGORY_NAME)]
        public string CaregoryName { get; set; }
    }
}
