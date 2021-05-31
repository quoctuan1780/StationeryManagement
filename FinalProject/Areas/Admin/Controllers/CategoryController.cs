using static Common.Constant;
using static Common.MessageConstant;
using Entities.Models;
using FinalProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Threading.Tasks;
using System.Transactions;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
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
                            ViewBag.MessageSuccess = MESSAGE_SUCCESS_ADD_CATEGORY;
                        }
                    }
                    else
                    {
                        ViewBag.MessageExists = MESSAGE_EXISTS_ADD_CATEGORY;
                    }
                }
                catch
                {
                    ViewBag.MessageError = MESSAGE_ERROR_ADD_CATEGORY;
                }
            }

            return View();
        }

        [HttpDelete]
        public async Task<int> DeleteCategory(int? categoryId)
        {
            if (categoryId is null)
            {
                return ERROR_CODE_NULL;
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var result = await _categoryService.DeleteCategoryByIdAsync(categoryId.Value);

                if (result > 0)
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

        [HttpPut]
        public async Task<int> RepairCategory(int? categoryId, string content)
        {
            if (categoryId is null || content is null)
            {
                return ERROR_CODE_NULL;
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var result = await _categoryService.UpdateCategoryByIdAsync(categoryId.Value, content);

                if (result > 0)
                {
                    transaction.Complete();

                    return CODE_SUCCESS;
                }
                else
                {
                    return CODE_FAIL;
                }
            }
            catch
            {
            }

            return ERROR_CODE_SYSTEM;
        }
    }
}
