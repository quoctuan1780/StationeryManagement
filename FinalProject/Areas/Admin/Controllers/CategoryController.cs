using Common;
using Entities.Models;
using FinalProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Threading.Tasks;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(Constant.AREA_ADMIN)]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();

            return View();
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var category = new Category()
                    {
                        CategoryName = model.CaregoryName
                    };

                    bool isExistsCategory = await _categoryService.IsExistsCategoryAsync(category);

                    if (isExistsCategory)
                    {
                        var result = await _categoryService.AddCategoryAsync(category);

                        if (!(result is null))
                        {
                            ViewBag.MessageSuccess = MessageConstant.MESSAGE_SUCCESS_ADD_CATEGORY;
                        }
                    }
                    else
                    {
                        ViewBag.MessageExists = MessageConstant.MESSAGE_EXISTS_ADD_CATEGORY;
                    }
                }
                catch
                {
                    ViewBag.MessageError = MessageConstant.MESSAGE_ERROR_ADD_CATEGORY;
                }
            }

            return View();
        }
    }
}
