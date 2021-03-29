using Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.Admin.ViewModels
{
    public class ReceiptViewModel
    {
        [Required(ErrorMessage = ValidationConstant.ERROR_CATEGORY_NAME_EMPTY)]
        [StringLength(30, ErrorMessage = ValidationConstant.ERROR_CATEGORY_NAME_RANGE)]
        [Display(Name = DisplayNameConstant.DISPLAY_CATEGORY_NAME)]
        public int ProductId { get; set; }

        public int ProviderId { get; set; }

        [Required (ErrorMessage = ValidationConstant.ERROR_PRODUCT_QUANTITY)]
        [Display(Name =DisplayNameConstant.DISPLAY_PRODUCT_QUANTITY)]
        [Range(1, int.MaxValue, ErrorMessage = ValidationConstant.ERROR_PRODUCT_QUANTITY)]
        public int Quantity { get; set; } = 0;

        [Required(ErrorMessage = ValidationConstant.ERROR_PRODUCT_PRICE_EMPTY)]
        [Display(Name = DisplayNameConstant.DISPLAY_PRODUCT_PRICE)]
        [Range(1, int.MaxValue, ErrorMessage = ValidationConstant.ERROR_PRODUCT_PRICE_RANGE)]
        public decimal ImportPrice { get; set; }

        public decimal Total { get; set; }

    }
}