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
        private IProductService _productService;

        public WarehouseController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }
        //public Task<IActionResult> RemainingQuantity()
        //{

        //}
    }
}
