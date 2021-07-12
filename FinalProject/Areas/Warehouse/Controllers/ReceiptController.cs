
﻿using Microsoft.AspNetCore.Hosting;
﻿using static Common.Constant;
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
