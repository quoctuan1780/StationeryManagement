using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.Warehouse.Controllers
{
    public class WarehouseController : Controller
    {
        private IProductDetailService _productDetailService;

        public WarehouseController(IProductDetailService productDetailService)
        {
            _productDetailService = productDetailService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> RemainingQuantity()
        {
            ViewBag.Remaining = await _productDetailService.GetListProductDetailAsync();
            return View();
        }
    }
}
