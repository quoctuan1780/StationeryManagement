using Common;
using Entities.Models;
using FinalProject.Areas.Admin.Helpers;
using FinalProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System;
using System.Threading.Tasks;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(RoleConstant.ROLE_ADMIN)]
    public class ProductController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;

        public ProductController(ICategoryService categoryService, IProductService productService, 
            IProductImageService productImageService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _productImageService = productImageService;
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
                try
                {
                    await ProductHelper.SaveImageAsync(model.Images);

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
                                ViewBag.MessageSuccess = MessageConstant.MESSAGE_SUCCESS_ADD_PRODUCT;
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

            return View();
        }

        public async Task<IActionResult> EditProduct(int id)
        {

            return View();
        }
    }
}
