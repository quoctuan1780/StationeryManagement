using static Common.ValidationConstant;
using static Common.DisplayNameConstant;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.Admin.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = ERROR_PRODUCT_NAME_EMPTY)]
        [StringLength(200)]
        [Display(Name = DISPLAY_PRODUCT_NAME)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = ERROR_PRODUCT_PRICE_EMPTY)]
        [Range(0, double.MaxValue, ErrorMessage = ERROR_PRODUCT_PRICE_RANGE)]
        [Display(Name = DISPLAY_PRODUCT_PRICE)]
        public decimal Price { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Display(Name = DISPLAY_PRODUCT_DESCRIPTION)]
        public string Description { get; set; }

        [Required(ErrorMessage = ERROR_CATEGORY_NAME_EMPTY)]
        [Display(Name = DISPLAY_PRODUCT_CATEGORY)]
        public int CategoryId { get; set; }

        [Display(Name = DISPLAY_PRODUCT_IMAGE)]
        public IList<IFormFile> Images { get; set; }

        public IList<string> ImagesString { get; set; }

        // Product detail
        public IList<int> ProductsDetailId { get; set; }
        public IList<string> Origins { get; set; }
        public IList<double> Weights { get; set; }
        public IList<int> Widths { get; set; }
        public IList<int> Lengths { get; set; }
        public IList<int> Heights { get; set; }
        public IList<string> Colors { get; set; }
        public IList<int> Quantities { get; set; }
    }
}
