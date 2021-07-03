using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using static Common.ValidationConstant;
using static Common.DisplayNameConstant;
using System.Collections.Generic;

namespace FinalProject.Areas.Admin.ViewModels
{
    public class SaleViewModel
    {
        public int SaleId { get; set; }

        [Required(ErrorMessage = ERROR_SALE_NAME_EMPTY)]
        [Display(Name = DISPLAY_NAME_SALE)]
        public string SaleName { get; set; }

        [Required(ErrorMessage = ERROR_DISCOUNT_EMPTY)]
        [Range(0, 100, ErrorMessage = ERROR_DISCOUNT_RANGE)]
        [Display(Name = DISPLAY_DISCOUNT)]
        public decimal Discount { get; set; }

        [Required(ErrorMessage = ERROR_SALE_TYPE_EMPTY)]
        [Display(Name = DISPLAY_TYPE_SALE)]
        public string SaleType { get; set; }

        [Display(Name = DISPLAY_SALE_DESCRIPTION)]
        public string Description { get; set; }

        [Required(ErrorMessage = ERROR_START_SALE_DATE_EMPTY)]
        [Display(Name = DISPLAY_START_DATE_SALE)]
        public DateTime SaleStartDate { get; set; }

        [Required(ErrorMessage = ERROR_END_SALE_DATE_EMPTY)]
        [Display(Name = DISPLAY_END_DATE_SALE)]
        public DateTime SaleEndDate { get; set; }

        public string Image { get; set; }

        [Display(Name = DISPLAY_IMAGE_SALE)]
        public IFormFile ImageFile { get; set; }

        [Required(ErrorMessage = "Bạn chưa chọn sản phẩm nào để áp dụng khuyến mại")]
        public IList<int> ProductIds { get; set; }
        [Required(ErrorMessage = "Giá tiền tối thiểu không được để trống")]
        public decimal FromOrderPrice { get; set; }
    }
}
