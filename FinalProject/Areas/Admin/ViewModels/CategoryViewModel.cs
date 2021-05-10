using static Common.ValidationConstant;
using static Common.DisplayNameConstant;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.Admin.ViewModels
{
    public class CategoryViewModel
    {
        [Required(ErrorMessage = ERROR_CATEGORY_NAME_EMPTY)]
        [StringLength(30, ErrorMessage = ERROR_CATEGORY_NAME_RANGE)]
        [Display(Name = DISPLAY_CATEGORY_NAME)]
        public string CaregoryName { get; set; }
    }
}
