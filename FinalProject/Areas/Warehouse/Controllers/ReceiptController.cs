using static Common.Constant;
using static Common.RoleConstant;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FinalProject.Areas.Warehouse.Controllers
{
    [Area(AREA_WAREHOUSE)]
    [Authorize(Roles = ROLE_WAREHOUSE_MANAGER, AuthenticationSchemes = ROLE_WAREHOUSE_MANAGER)]
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
