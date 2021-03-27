using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Detail(int id)
        {

            ViewBag.Product = await _productService.GetProductByIdAsync(id);

            return View();
        }
    }
}
