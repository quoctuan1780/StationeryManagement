using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfacies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public IRecommendationService _recommandationService;

        public HomeController(IProductService productService, IRecommendationService recommendationService)
        {
            _productService = productService;
            _recommandationService = recommendationService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Products = await _productService.GetAllProductsAsync();

            return View();
        }

       
    }
}
