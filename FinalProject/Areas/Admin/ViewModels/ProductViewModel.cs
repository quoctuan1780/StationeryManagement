using Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.Admin.ViewModels
{
    public class ProductViewModel
    {
        [Required(ErrorMessage = ValidationConstant.ERROR_PRODUCT_NAME_EMPTY)]
        [StringLength(200)]
        [Display(Name = DisplayNameConstant.DISPLAY_PRODUCT_NAME)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = ValidationConstant.ERROR_PRODUCT_PRICE_EMPTY)]
        [Range(0, double.MaxValue, ErrorMessage = ValidationConstant.ERROR_PRODUCT_PRICE_RANGE)]
        [Display(Name = DisplayNameConstant.DISPLAY_PRODUCT_PRICE)]
        public decimal Price { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Display(Name = DisplayNameConstant.DISPLAY_PRODUCT_DESCRIPTION)]
        public string Description { get; set; }

        [Required(ErrorMessage = ValidationConstant.ERROR_CATEGORY_NAME_EMPTY)]
        [Display(Name = DisplayNameConstant.DISPLAY_PRODUCT_CATEGORY)]
        public int CategoryId { get; set; }

        [Display(Name = DisplayNameConstant.DISPLAY_PRODUCT_IMAGE)]
        public IList<IFormFile> Images { get; set; }
    }
}
