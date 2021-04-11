using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.Admin.ViewModels
{
    public class ReceiptRequestViewModel
    {
        public int ReceiptRequestId { get; set; }

       
        [Display(Name = DisplayNameConstant.DISPLAY_RECEIPT_REQUEST_CREATE_DATE)]
        public DateTime CreateDate { get; set; }

        [Display(Name = DisplayNameConstant.DISPLAY_RECEIPT_REQUEST_USER_ID)]
        public string UserId { get; set; }

        [Display(Name = DisplayNameConstant.DISPLAY_RECEIPT_REQUEST_STATUS)]
        public string Status { get; set; }

        [Required]
        [Display(Name = DisplayNameConstant.DISPLAY_PRODUCT_NAME)]
        public IList<int> ProductDetailId { get; set; }

        [Required]
        [Display(Name = DisplayNameConstant.DISPLAY_PRODUCT_COLOR)]
        public IList<string> Color { get; set; }

        [Required]
        [Display(Name = DisplayNameConstant.DISPLAY_PRODUCT_SIZE)]
        public IList<string> Size { get; set; }


        [Required]
        [Display(Name = DisplayNameConstant.DISPLAY_PRODUCT_ORIGIN)]
        public IList<string> Origin { get; set; }


        [Required]
        [Display(Name = DisplayNameConstant.DISPLAY_PRODUCT_QUANTITY)]
        public IList<int> Quantity { get; set; } 

    }
}
