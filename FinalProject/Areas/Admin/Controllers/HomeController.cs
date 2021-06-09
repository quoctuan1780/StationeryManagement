using static Common.Constant;
using static Common.RoleConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Services.Interfacies;
using System.Threading.Tasks;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
    [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
    public class HomeController : Controller
    {
        private readonly IHubService _hubService;

        public HomeController(IHubService hubService)
        {
            _hubService = hubService;
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> GetOrderQuantity()
        {
            return Ok(await _hubService.GetOrdersAsync());
        }

        public async Task<IActionResult> GetWarehouseWait()
        {
            return Ok(await _hubService.GetWarehouseAsync());
        }
        
        public async Task<IActionResult> GetAccount()
        {
            return Ok(await _hubService.GetAccountAsync());
        }

        public async Task<IActionResult> GetRevenue()
        {
            return Ok(await _hubService.GetRevenueAsync());
        }
        public async Task<IActionResult> GetTopProduct()
        {
            return Ok(await _hubService.GetTopProductAsync());
        }
        public async Task<IActionResult> GetRevenueCurrentMonth()
        {
            return Ok(await _hubService.GetRevenueCurrentMonthAsync());
        }
    }
}
