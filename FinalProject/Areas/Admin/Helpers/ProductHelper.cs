﻿using Common;
using Entities.Models;
using FinalProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;

namespace FinalProject.Areas.Admin.Helpers
{
    public class ProductHelper
    {
        public static ProductViewModel RemoveProductsDetailOnProductDetailsViewModel(ProductViewModel model, 
            IList<string> productsDetailId)
        {
            if (productsDetailId.FirstOrDefault() is null) return model;

            var productsDetailIdConvert = productsDetailId.FirstOrDefault().Split(Constant.COMMA);

            foreach(var item in productsDetailIdConvert)
            {
                var checkParseInt = int.TryParse(item, out int resultParse);

                if (!checkParseInt) return null;

                var index = model.ProductsDetailId.IndexOf(resultParse);

                model.Widths.RemoveAt(index);
                model.Weights.RemoveAt(index);
                model.Lengths.RemoveAt(index);
                model.Heights.RemoveAt(index);
                model.Colors.RemoveAt(index);
                model.Quantities.RemoveAt(index);
                model.Origins.RemoveAt(index);
                model.ProductsDetailId.RemoveAt(index);
            }

            return model;
        }

        public static IList<ProductDetail> ConvertModelPrductToProductsDetail(ProductViewModel model, int? productId)
        {
            if (model.Origins is null || productId is null) return null;

            var productsDetail = new List<ProductDetail>();

            for(int i = 0; i < model.Origins.Count; i++)
            {
                productsDetail.Add(new ProductDetail()
                {
                    Color = model.Colors[i],
                    Height = model.Heights[i],
                    Length = model.Lengths[i],
                    Weight = model.Weights[i],
                    Origin = model.Origins[i],
                    Quantity = model.Quantities[i],
                    Width = model.Widths[i],
                    ProductId = productId.Value
                });
            }

            if (!(model.ProductsDetailId is null))
            {
                for (int i = 0; i < model.ProductsDetailId.Count; i++)
                {
                    productsDetail[i].ProductDetailId = model.ProductsDetailId[i];
                }
            }
            
            return productsDetail;
        }
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

        public static async Task<IList<string>> SaveImageAsync(IList<IFormFile> formFiles, int width, int height)
        {
            if (formFiles is null) return null;

            var filesName = new List<string>();

            foreach (var item in formFiles)
            {
                var image = Image.Load(item.OpenReadStream());

                image.Mutate(x => 
                    x.Resize(image.Width > width ? width : image.Width, image.Height > height ? height : image.Height));

                var resultHash = HashNameImage(item.FileName);

                filesName.Add(resultHash);

                await image.SaveAsync(Constant.IMAGE_LINK + resultHash);
            }

            return filesName;
        }

        public static string HashNameImage(string name)
        {
            byte[] salt;
            byte[] buffer;

            using (var hashName = new Rfc2898DeriveBytes(name, 0x10, 0x3e88))
            {
                salt = hashName.Salt;
                buffer = hashName.GetBytes(0x20);
            }

            var dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer, 0, dst, 0x11, 0x20);

            string resultHash = Convert.ToBase64String(dst).ToString();

            resultHash = resultHash.Replace("/", "");
            resultHash = resultHash.Replace("+", "");
            resultHash = resultHash.Replace("==", "");

            resultHash += name;

            return resultHash;
        }

        public static IList<ProductImage> CreateProductImages(IList<string> imagesName, int? productId)
        {
            if (imagesName is null || productId is null) return null;

            var productImages = new List<ProductImage>();

            foreach(var item in imagesName)
            {
                productImages.Add(new ProductImage()
                {
                    ProductId = productId.Value,
                    Image = item,
                    PrimaryImage = false
                });
            }

            return productImages;
        }

        public static ProductViewModel ConvertProductToProductViewModel(Product product)
        {
            if (product is null) return null;

            var model = new ProductViewModel()
            {
                CategoryId = product.CategoryId,
                ProductName = product.ProductName,
                Price = product.Price,
                CreateDate = product.DateCreate,
                Description = HttpUtility.HtmlDecode(product.Description),
                ProductId = product.ProductId
            };

            var productsDetailId = new List<int>();
            var images = new List<string>();
            var origins = new List<string>();
            var weights = new List<double>();
            var widths = new List<int>();
            var lengths = new List<int>();
            var heights = new List<int>();
            var color = new List<string>();
            var quantities = new List<int>();

            foreach (var item in product.ProductImages)
            {
                images.Add(item.Image);
            }

            for(int i = 0; i < product.ProductDetails.Count; i++)
            {
                origins.Add(product.ProductDetails[i].Origin);
                weights.Add(product.ProductDetails[i].Weight);
                widths.Add(product.ProductDetails[i].Width);
                lengths.Add(product.ProductDetails[i].Length);
                heights.Add(product.ProductDetails[i].Height);
                color.Add(product.ProductDetails[i].Color);
                productsDetailId.Add(product.ProductDetails[i].ProductDetailId);
                quantities.Add(product.ProductDetails[i].Quantity);
            }

            model.ProductsDetailId = productsDetailId;
            model.ImagesString = images;
            model.Origins = origins;
            model.Weights = weights;
            model.Widths = widths;
            model.Heights = heights;
            model.Lengths = lengths;
            model.Colors = color;
            model.Quantities = quantities;

            return model;
        }
    }
}
