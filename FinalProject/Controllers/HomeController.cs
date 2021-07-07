using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ISaleService _saleService;
        public IRecommendationService _recommandationService;

        public HomeController(IProductService productService, ISaleService saleService, IRecommendationService recommendationService)
        {
            _productService = productService;
            _saleService = saleService;
            _recommandationService = recommendationService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Products = await _productService.GetAllProductsAsync();

            ViewBag.Sales = await _saleService.GetThreeSalesImageAsync();

            return View();
        }


    }
}
