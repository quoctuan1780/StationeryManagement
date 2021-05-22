using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Threading.Tasks;

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
    }
}
