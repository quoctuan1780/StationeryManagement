using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Threading.Tasks;
using static Common.Constant;

namespace FinalProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICommentService _commentService;

        public ProductController(IProductService productService, ICommentService commentService)
        {
            _productService = productService;
            _commentService = commentService;
        }
        public async Task<IActionResult> Detail(int id)
        {

            ViewBag.Product = await _productService.GetProductByIdAsync(id);

            ViewBag.Comments = await _commentService.GetAllCommentsByProductIdAsync(id);

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
