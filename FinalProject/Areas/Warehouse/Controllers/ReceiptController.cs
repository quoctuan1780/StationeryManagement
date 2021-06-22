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

        public ReceiptController(IProductService productService, IReceiptService receiptService)
        {
            _productService = productService;
            _receiptService = receiptService;
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
