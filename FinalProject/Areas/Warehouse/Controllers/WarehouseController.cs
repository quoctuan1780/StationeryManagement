using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Threading.Tasks;
using static Common.Constant;
using static Common.RoleConstant;

namespace FinalProject.Areas.Warehouse.Controllers
{
    [Area(AREA_WAREHOUSE)]
    [Authorize(Roles = ROLE_ADMIN_WAREHOUSE_MANAGER, AuthenticationSchemes = ROLE_ADMIN_WAREHOUSE_MANAGER)]
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
