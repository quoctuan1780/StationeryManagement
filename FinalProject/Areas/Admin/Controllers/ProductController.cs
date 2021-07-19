using Entities.Models;
using FinalProject.Areas.Admin.Helpers;
using FinalProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using static Common.Constant;
using static Common.MessageConstant;
using static Common.RoleConstant;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
    [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
    public class ProductController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;
        private readonly IProductDetailService _productDetailService;

        public ProductController(ICategoryService categoryService, IProductService productService,
            IProductImageService productImageService, IProductDetailService productDetailService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _productImageService = productImageService;
            _productDetailService = productDetailService;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Products = await _productService.GetProductsAsync();

            return View();
        }

        public async Task<IActionResult> ProductDetail(int? productId)
        {
            if (productId is null)
            {
                return PartialView(ERROR_404_PAGE_ADMIN);
            }

            try
            {
                ViewBag.ProductDetail = await _productDetailService.GetProductsDetailByProductIdAsync(productId.Value);

                return View();
            }
            catch
            { 
            }

            return PartialView(ERROR_404_PAGE_ADMIN);
        }

        public async Task<IActionResult> AddProduct()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            ViewBag.Categories = ProductHelper.ConvertCategoriesToSelectListItem(categories);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductViewModel model, string imagePreviewDeteted)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            ViewBag.Categories = ProductHelper.ConvertCategoriesToSelectListItem(categories);

            if (ModelState.IsValid)
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                try
                {
                    if (!(imagePreviewDeteted is null))
                    {
                        var imagesPreviewDeleted = imagePreviewDeteted.Split(COMMA).ToArray();
                        if (!(model.Images is null))
                        {
                            int i = 0;
                            while (i < model.Images.Count)
                            {
                                if (imagesPreviewDeleted.Contains(model.Images[i].FileName))
                                {
                                    model.Images.Remove(model.Images[i]);
                                }
                                else
                                {
                                    i++;
                                }
                            }
                        }
                    }

                    var imagesName = new List<string>();


                    var fileNames = await ProductHelper.SaveImageAsync(model.Images, 1920, 1080);

                    var product = new Product()
                    {
                        ProductName = model.ProductName,
                        DateCreate = DateTime.Now,
                        Description = model.Description,
                        Price = model.Price,
                        CategoryId = model.CategoryId
                    };

                    if (!(await _productService.IsExistsProduct(product)))
                    {
                        product = await _productService.AddProductAsync(product);

                        if (!(product is null))
                        {
                            var result = await _productImageService.AddListImagesAsync(
                                ProductHelper.CreateProductImages(fileNames, product.ProductId));

                            if (result > 0)
                            {
                                var productsDetail =
                                    ProductHelper.ConvertModelPrductToProductsDetail(model, product.ProductId);

                                result = await _productDetailService.AddProductsDetailAsync(productsDetail);

                                if (result > 0)
                                {
                                    TempData["MessageSuccess"] = MESSAGE_SUCCESS_ADD_PRODUCT;

                                    transaction.Complete();

                                    return Redirect("/Admin/Product/Index");
                                }
                                else if (result == ERROR_CODE_NULL)
                                {
                                    ViewBag.MessageError = MESSAGE_ERROR_ADD_PRODUCT_DETAIL;
                                }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.MessageExists = MESSAGE_EXISTS_ADD_PRODUCT;
                    }
                }
                catch
                {
                    ViewBag.MessageError = MESSAGE_ERROR_ADD_PRODUCT;
                }
            }

            return View(model);
        }

        [HttpDelete]
        public async Task<int> DeleteProduct(int? productId)
        {
            if(productId is null)
            {
                return ERROR_CODE_NULL;
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var result = await _productService.DeleteProductByIdAsync(productId.Value);

                if(result > 0)
                {
                    transaction.Complete();

                    return CODE_SUCCESS;
                }
            }
            catch
            {
            }

            return ERROR_CODE_SYSTEM;
        }

        public async Task<IActionResult> EditProduct(int id, string urlCallback)
        {
            var product = await _productService.GetProductByIdAsync(id);

            var categories = await _categoryService.GetAllCategoriesAsync();

            ViewBag.Categories = ProductHelper.ConvertCategoriesToSelectListItem(categories);

            if (product is null)
            {
                return Redirect(urlCallback);
            }

            ProductViewModel model = ProductHelper.ConvertProductToProductViewModel(product, null, null);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(ProductViewModel model, IList<string> imageRemove,
            IList<string> productsDetailsId, string imagePreviewDeteted)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            ViewBag.Categories = ProductHelper.ConvertCategoriesToSelectListItem(categories);

            int result;

            if (ModelState.IsValid)
            {
                try
                {
                    if (!(imagePreviewDeteted is null))
                    {
                        var imagesPreviewDeteted = imagePreviewDeteted.Split(COMMA).ToArray();
                        if (!(model.Images is null))
                        {
                            int i = 0;
                            while (i < model.Images.Count)
                            {
                                if (imagePreviewDeteted.Contains(model.Images[i].FileName))
                                {
                                    model.Images.Remove(model.Images[i]);
                                }
                                else
                                {
                                    i++;
                                }
                            }
                        }
                    }
                    using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                    result = await _productImageService.DeleteListImagesOfProductByNameAsync(imageRemove, model.ProductId);

                    // delete images successfully
                    if (result > 0)
                    {
                        result = await _productDetailService.DeleteProductsDetailAsync(productsDetailsId, model.ProductId);

                        // delete products detail successfully
                        if (result > 0)
                        {
                            var product = new Product()
                            {
                                ProductName = model.ProductName,
                                DateCreate = model.CreateDate,
                                Description = model.Description,
                                Price = model.Price,
                                CategoryId = model.CategoryId,
                                ProductId = model.ProductId
                            };

                            product = await _productService.UpdateProductAsync(product);

                            if (!(product is null))
                            {

                                result = await _productDetailService.UpdateProductsDetailAsync(
                                    ProductHelper.ConvertModelPrductToProductsDetailForUpdate(model, product.ProductId));

                                if (result > 0)
                                {
                                    // if user added images
                                    if (!(model.Images is null))
                                    {
                                        var imagesName = await ProductHelper.SaveImageAsync(model.Images, 1920, 1080);

                                        await _productImageService.AddListImagesAsync(
                                            ProductHelper.CreateProductImages(imagesName, product.ProductId));
                                    }

                                    TempData["MessageSuccess"] = MESSAGE_SUCCESS_UPDATE_PRODUCT;

                                    transaction.Complete();

                                    return Redirect("/Admin/Product/Index");
                                }
                                else if (result == ERROR_CODE_NULL)
                                {
                                    ViewBag.MessageSuccess = MESSAGE_ERROR_UPDATE_PRODUCT_DETAIL;
                                }
                            }
                            else
                            {
                                ViewBag.MessageError = MESSAGE_ERROR_UPDATE_PRODUCT_NULL;
                            }
                        }
                    }
                    else if (result == ERROR_CODE_CONVERT_TO_INT)
                    {
                        ViewBag.MessageError = MESSAGE_ERROR_CONVERT_TO_INT;
                    }
                }
                catch
                {
                    ViewBag.MessageError = MESSAGE_ERROR_UPDATE_PRODUCT;
                }
            }

            model = ProductHelper.ConvertProductToProductViewModel(
                        await _productService.GetProductByIdAsync(model.ProductId), imageRemove, productsDetailsId);

            return View(model);
        }
    }
}
