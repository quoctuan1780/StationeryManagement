using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Common.RoleConstant;
using static Common.Constant;
using System.Threading.Tasks;
using Services.Interfacies;
using Entities.Models;

namespace FinalProject.Areas.Shipper.Controllers
{
    [Area(AREA_SHIPPER)]
    [Authorize(Roles = ROLE_SHIPPER, AuthenticationSchemes = ROLE_SHIPPER)]
    public class HomeController : Controller
    {
        private readonly IHubShipperService _hubShipperService;
        private readonly IAccountService _accountService;

        public HomeController(IHubShipperService hubShipperService, IAccountService accountService)
        {
            _hubShipperService = hubShipperService;
            _accountService = accountService;
        }
        public IActionResult Dashboard()
        {
            return View();
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
