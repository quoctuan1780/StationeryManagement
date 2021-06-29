using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Common.Constant;

namespace FinalProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICommentService _commentService;
        private readonly IRecommendationService _recommendationService;

        public ProductController(IProductService productService, ICommentService commentService, IRecommendationService recommendationService)
        {
            _productService = productService;
            _commentService = commentService;
            _recommendationService = recommendationService;
        }
        public async Task<IActionResult> Detail(int id)
        {

            ViewBag.Product = await _productService.GetProductByIdAsync(id);

            ViewBag.Comments = await _commentService.GetAllCommentsByProductIdAsync(id);
       
            List<int> listProductDetailId = await _productService.GetProductDetailByProDuctIdAsync(id);
            ViewBag.Suggest = await _recommendationService.GetSuggestedProduct(listProductDetailId);
            

            return View();
        }

        public string GetProductSkip(int? skip)
        {
            if(skip is null)
            {
                return ERROR_CODE_NULL.ToString();
            }

            try
            {
                return _productService.GetProductSkip(skip.Value);
            }
            catch
            {

            }

            return ERROR_CODE_SYSTEM.ToString();
        }
    }
}
