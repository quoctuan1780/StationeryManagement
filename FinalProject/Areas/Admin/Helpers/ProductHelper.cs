﻿using Common;
using Entities.Models;
using FinalProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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

        public static void RemoveFile(IList<string> images)
        {
            if (images.FirstOrDefault() is null) return;

            var imagesRemove = images.FirstOrDefault().Split(Constant.COMMA);

            foreach (var item in imagesRemove)
            {
                File.Delete(Constant.IMAGE_LINK + item);
            }
        }

        public static async Task SaveImageAsync(IList<IFormFile> formFiles, int width, int height)
        {
            if (formFiles is null) return;

            foreach (var item in formFiles)
            {
                var image = Image.Load(item.OpenReadStream());

                image.Mutate(x => 
                    x.Resize(image.Width > width ? width : image.Width, image.Height > height ? height : image.Height));

                await image.SaveAsync(Constant.IMAGE_LINK + item.FileName);
            }
        }

        public static IList<ProductImage> CreateProductImages(IList<IFormFile> formFiles, int productId)
        {
            if (formFiles is null) return null;

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

        public static ProductViewModel ConvertProductToProductViewModel(Product product)
        {
            var model = new ProductViewModel()
            {
                CategoryId = product.CategoryId,
                ProductName = product.ProductName,
                Price = product.Price,
                CreateDate = product.DateCreate,
                Description = HttpUtility.HtmlDecode(product.Description),
                ProductId = product.ProductId
            };

            var images = new List<string>();

            foreach(var item in product.ProductImages)
            {
                images.Add(item.Image);
            }

            model.ImagesString = images;

            return model;
        }
    }
}