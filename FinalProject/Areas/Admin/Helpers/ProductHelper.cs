using Common;
using Entities.Models;
using FinalProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FinalProject.Areas.Admin.Helpers
{
    public class ProductHelper
    {
        public static IList<SelectListItem> ConvertCategoriesToSelectListItem(IList<Category> categories)
        {
            var categoriesSelectList = new List<SelectListItem>();

            foreach (var item in categories)
            {
                categoriesSelectList.Add(new SelectListItem()
                {
                    Text = item.CategoryName,
                    Value = item.CategoryId.ToString()
                });
            }

            return categoriesSelectList;
        }

        public static async Task SaveImageAsync(IList<IFormFile> images)
        {
            foreach(var item in images)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), Constant.IMAGE_LINK, item.FileName);
                var stream = new FileStream(path, FileMode.Create);
                await item.CopyToAsync(stream);
            }
        }

        public static IList<ProductImage> CreateProductImages(IList<IFormFile> formFiles, int productId)
        {
            var productImages = new List<ProductImage>();
            foreach(var item in formFiles)
            {
                productImages.Add(new ProductImage()
                {
                    ProductId = productId,
                    Image = item.FileName,
                    PrimaryImage = false
                });
            }

            return productImages;
        }

        public ProductViewModel ConvertProductToProductViewModel(Product product)
        {
            var model = new ProductViewModel()
            {
                CategoryId = product.CategoryId,
                ProductName = product.ProductName,
                Price = product.Price,
                CreateDate = product.DateCreate,
                Description = product.Description
            };

            return model;
        }
    }
}
