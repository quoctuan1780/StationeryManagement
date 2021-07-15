using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Common.DisplayNameConstant;

namespace FinalProject.Areas.Warehouse.ViewModels
{
    public class ReceiptRequestViewModel
    {
        public int ReceiptRequestId { get; set; }

        [Display(Name = DISPLAY_RECEIPT_REQUEST_CREATE_DATE)]
        public DateTime CreateDate { get; set; }

        [Display(Name = DISPLAY_RECEIPT_REQUEST_USER_ID)]
        public string UserId { get; set; }

        [Display(Name = DISPLAY_RECEIPT_REQUEST_STATUS)]
        public string Status { get; set; }

        [Required]
        [Display(Name = DISPLAY_PRODUCT_NAME)]
        public IList<int> ProductDetailId { get; set; }

        [Display(Name = DISPLAY_PRODUCT_QUANTITY)]
        public IList<int> Quantity { get; set; }
        public IList<decimal> Prices { get; set; }
    }
}
