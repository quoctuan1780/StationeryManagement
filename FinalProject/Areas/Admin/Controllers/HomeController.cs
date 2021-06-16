using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System;
using System.Threading.Tasks;
using X.PagedList;
using static Common.Constant;
using static Common.RoleConstant;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
    [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
    public class HomeController : Controller
    {
        private readonly IHubService _hubService;
        private readonly IAccountService _accountService;
        private readonly IWorkflowHistoryService _workflowHistoryService;

        public HomeController(IHubService hubService, IWorkflowHistoryService workflowHistoryService, IAccountService accountService)
        {
            _hubService = hubService;
            _accountService = accountService;
            _workflowHistoryService = workflowHistoryService;
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> ActivityLog(int? page = 1)
        {
            var user = await _accountService.GetUserAsync(User);

            var workFlow = await _workflowHistoryService.GetWorkflowHistoriesAsync(user.Id);

            var model = workFlow.ToPagedList(page.Value, 10);

            return View(model);
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
