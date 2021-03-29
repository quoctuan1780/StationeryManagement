using Common;
using Entities.Models;
using FinalProject.Areas.Admin.Helpers;
using FinalProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(RoleConstant.ROLE_ADMIN)]
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
            ViewBag.Products = await _productService.GetAllProductsAsync();

            return View();
        }
        public async Task<IActionResult> AddProduct()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            ViewBag.Categories = ProductHelper.ConvertCategoriesToSelectListItem(categories);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductViewModel model)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            ViewBag.Categories = ProductHelper.ConvertCategoriesToSelectListItem(categories);

            if (ModelState.IsValid)
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        await ProductHelper.SaveImageAsync(model.Images, 400, 400);

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
                                    ProductHelper.CreateProductImages(model.Images, product.ProductId));

                                if (result > 0)
                                {
                                    var productsDetail = 
                                        ProductHelper.ConvertModelPrductToProductsDetail(model, product.ProductId);

                                    result = await _productDetailService.AddProductsDetailAsync(productsDetail);

                                    if (result > 0)
                                    {
                                        ViewBag.MessageSuccess = MessageConstant.MESSAGE_SUCCESS_ADD_PRODUCT;

                                        transaction.Complete();
                                    }
                                }
                            }
                        }
                        else
                        {
                            ViewBag.MessageExists = MessageConstant.MESSAGE_EXISTS_ADD_PRODUCT;
                        }
                    }
                    catch
                    {
                        ViewBag.MessageError = MessageConstant.MESSAGE_ERROR_ADD_PRODUCT;
                    }
                }
            }

            return View(model);
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

            ProductViewModel model = ProductHelper.ConvertProductToProductViewModel(product);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(ProductViewModel model, IList<string> imageRemove)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            ViewBag.Categories = ProductHelper.ConvertCategoriesToSelectListItem(categories);

            if (ModelState.IsValid)
            {
                try
                {
                    ProductHelper.RemoveFile(imageRemove);

                    var result = await _productImageService.DeleteListImagesOfProductByNameAsync(
                        imageRemove, 
                        model.ProductId);

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
                            if (!(model.Images is null))
                            {
                                await ProductHelper.SaveImageAsync(model.Images, 400, 400);

                                await _productImageService.AddListImagesAsync(
                                    ProductHelper.CreateProductImages(model.Images, product.ProductId));

                                if (result > 0)
                                {
                                    ViewBag.MessageSuccess = MessageConstant.MESSAGE_SUCCESS_UPDATE_PRODUCT;
                                }
                            }
                            else
                            {
                                ViewBag.MessageSuccess = MessageConstant.MESSAGE_SUCCESS_UPDATE_PRODUCT;
                            }
                        }
                    }
                    
                }
                catch
                {
                    ViewBag.MessageError = MessageConstant.MESSAGE_ERROR_UPDATE_PRODUCT;
                }
            }

            var productAfterUpdate = await _productService.GetProductByIdAsync(model.ProductId);

            model.ImagesString =
                productAfterUpdate.ProductImages.Select(x => x.Image).ToList();

            model.Description = HttpUtility.HtmlDecode(model.Description);

            return View(model);
        }
    }
}
