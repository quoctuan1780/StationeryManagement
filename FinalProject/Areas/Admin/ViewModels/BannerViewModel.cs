using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using static Common.ValidationConstant;
using static Common.DisplayNameConstant;

namespace FinalProject.Areas.Admin.ViewModels
{
    public class BannerViewModel
    {
        public int BannerId { get; set; }

        [Required(ErrorMessage = ERROR_BANNER_IMAGE_EMPTY)]
        [Display(Name = DISPLAY_BANNER_IMAGE)]
        public IFormFile Image { get; set; }

        public string ImageString { get; set; }

        [Required(ErrorMessage = ERROR_BANNER_START_DATE_EMPTY)]
        [Display(Name = DISPLAY_BANNER_START_DATE)]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = ERROR_BANNER_END_DATE_EMPTY)]
        [Display(Name = DISPLAY_BANNER_END_DATE)]
        public DateTime EndDate { get; set; }

        [Display(Name = DISPLAY_BANNER_URL)]
        public string Url { get; set; } = "Default";
    }
}
