using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        

        public HomeController(IProductService productService)
        {
            _productService = productService;
            
        }
        
        public async Task<IActionResult> Index()
        {
            ViewBag.Products = await _productService.GetAllProductsAsync();

            return View();
        }
    }
}
