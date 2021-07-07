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
        private readonly IRatingService _rateService;

        public ProductController(IProductService productService, ICommentService commentService, IRatingService rateService,IRecommendationService recommendationService)
        {
            _productService = productService;
            _commentService = commentService;
            _rateService = rateService;
            _recommendationService = recommendationService;
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if(id is null)
            {
                return PartialView(ERROR_404_PAGE);
            }
            ViewBag.Product = await _productService.GetProductByIdAsync(id.Value);

            ViewBag.Comments = await _commentService.GetAllCommentsByProductIdAsync(id.Value);

            ViewBag.Ratings = await _rateService.GetRatingsAsync();

            List<int> listProductDetailId = await _productService.GetProductDetailByProDuctIdAsync(id.Value);
            ViewBag.Suggest = await _recommendationService.GetSuggestedProduct(listProductDetailId);
            
            ViewBag.RatingsDetail = await _rateService.GetRatingsDetailAsync(id.Value);

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
