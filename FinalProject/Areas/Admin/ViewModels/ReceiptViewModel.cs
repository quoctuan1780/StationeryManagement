using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Common.DisplayNameConstant;
using static Common.ValidationConstant;

namespace FinalProject.Areas.Admin.ViewModels
{
    public class ReceiptViewModel
    {

        public int ImportWarehouseId { get; set; }

        public DateTime CreateDate { get; set; }

        public string UserId { get; set; }

        public string Status { get; set; }


        [Required]
        [Display(Name = DISPLAY_RECEIPT_REQUEST)]
        public int ReceiptRequestId { get; set; }


        [Required(ErrorMessage = ERROR_CATEGORY_NAME_EMPTY)]
        [StringLength(30, ErrorMessage = ERROR_CATEGORY_NAME_RANGE)]
        [Display(Name = DISPLAY_CATEGORY_NAME)]
        public IList<int> ProductId { get; set; }

        public IList<int> ProviderId { get; set; }

        [Required(ErrorMessage = ERROR_PRODUCT_QUANTITY)]
        [Display(Name = DISPLAY_PRODUCT_QUANTITY)]
        [Range(1, int.MaxValue, ErrorMessage = ERROR_PRODUCT_QUANTITY)]
        public IList<int> Quantity { get; set; }

        [Required(ErrorMessage = ERROR_PRODUCT_PRICE_EMPTY)]
        [Display(Name = DISPLAY_PRODUCT_PRICE)]
        [Range(1, int.MaxValue, ErrorMessage = ERROR_PRODUCT_PRICE_RANGE)]
        public IList<decimal> ImportPrice { get; set; }

        [Required(ErrorMessage = ERROR_PRODUCT_PRICE_EMPTY)]
        [Display(Name = DISPLAY_PRODUCT_PRICE)]
        [Range(1, int.MaxValue, ErrorMessage = ERROR_PRODUCT_PRICE_RANGE)]
        public IList<int> ActualQuantity { get; set; }


        public IList<DateTime> ImportDate { get; set; }

        public decimal Total { get; set; }

    }
}