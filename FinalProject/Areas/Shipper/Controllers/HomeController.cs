using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Services.Interfacies;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Common.Constant;
using static Common.RoleConstant;

namespace FinalProject.Areas.Shipper.Controllers
{
    [Area(AREA_SHIPPER)]
    [Authorize(Roles = ROLE_SHIPPER, AuthenticationSchemes = ROLE_SHIPPER)]
    public class HomeController : Controller
    {
        private readonly IHubShipperService _hubShipperService;
        private readonly IAccountService _accountService;
        private readonly IWorkflowHistoryService _workflowHistoryService;

        public HomeController(IHubShipperService hubShipperService, IAccountService accountService, IWorkflowHistoryService workflowHistoryService)
        {
            _hubShipperService = hubShipperService;
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

            var model = new PagedList<WorkflowHistory>(workFlow, page.Value, 10);

            return View(model);
        }

        public async Task<IActionResult> GetOrderWaitForPick()
        {
            return Ok(await _hubShipperService.GetOrderWaitToPickAsync());
        }

        public async Task<IActionResult> GetOrderDelivering()
        {
            var user = await _accountService.GetUserAsync(User);
            return Ok(await _hubShipperService.GetOrderDeliveringAsync(user.Id));
        }

        public async Task<IActionResult> GetOrderDelivered()
        {
            var user = await _accountService.GetUserAsync(User);
            return Ok(await _hubShipperService.GetOrderDeliveredAsync(user.Id));
        }

        public async Task<IActionResult> GetOrderWaitToConfirmDelivery()
        {
            var user = await _accountService.GetUserAsync(User);
            return Ok(await _hubShipperService.GetOrderWaitToConfirmDeliveryAsync(user.Id));
        }

        public async Task<IActionResult> GetOrderDeliveringOrDelivered()
        {
            var user = await _accountService.GetUserAsync(User);
            return Ok(await _hubShipperService.GetOrderDeliveringOrDeliveriedAsync(user.Id));
        }
    }
}
