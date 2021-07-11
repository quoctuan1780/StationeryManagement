using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.Areas.Warehouse.Controllers
{
    public class ReceiptController : Controller
    {
        private readonly IProductService _productService;
        private readonly IReceiptService _receiptService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReceiptController(IProductService productService, IReceiptService receiptService, IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _receiptService = receiptService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
     
        public async Task<IActionResult> AutoCreateReceiptRequest()
        {
            ViewBag.ProductOutOfStock = await _productService.GetProductDetailsRunOutOfStockAsync();

            return View();
        }

    }
}
