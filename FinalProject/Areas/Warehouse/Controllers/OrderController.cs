using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Services.Hubs;
using Services.Interfacies;
using System;
using System.Threading.Tasks;
using System.Transactions;
using static Common.Constant;
using static Common.RoleConstant;
using static Common.SignalRConstant;


namespace FinalProject.Areas.Warehouse.Controllers
{
    [Area(AREA_WAREHOUSE)]
    [Authorize(Roles = ROLE_WAREHOUSE_MANAGER, AuthenticationSchemes = ROLE_WAREHOUSE_MANAGER)]
    public class OrderController : Controller
    {
        private readonly IHubContext<SignalServer> _hubContext;
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;
        private IWorkflowHistoryService _workflowHistoryService;

        public OrderController(IOrderService orderService, IWorkflowHistoryService workflowHistoryService, IAccountService accountService, IHubContext<SignalServer> hubContext)
        {
            _hubContext = hubContext;
            _orderService = orderService;
            _accountService = accountService;
            _workflowHistoryService = workflowHistoryService;
        }

        public async Task<IActionResult> OrderDetail(int? orderId)
        {
            if (orderId is null)
            {
                return PartialView(ERROR_404_PAGE_WAREHOUSE);
            }

            ViewBag.Order = await _orderService.GetOrderByIdAsync(orderId.Value);

            return View();
        }

        public async Task<int> PrepareOrder(int OrderId, int ProductId)
        {
            return await _orderService.PrepareOrder(OrderId,ProductId);
        }
        public async Task<int> RejectOrder(int id)
        {
            return await _orderService.RejectOrder(id);
        }
        public async Task<IActionResult> OrderWaitExportWarehouse(string customer = EMPTY, string orderDate = EMPTY)
        {
            ViewBag.Customers = await _accountService.GetAllCustomersAsync();
            if (customer != EMPTY || orderDate != EMPTY)
            {
                ViewBag.Orders = await _orderService.GetOrdersWaitExportWarehouseAsync(customer, orderDate);
            }
            else
            {
                ViewBag.Orders = await _orderService.GetOrdersWaitExportWarehouseAsync();
            }
            return View();
        }

        [HttpPut]
        public async Task<int> WarehouseManagementComfirmOrder(int? orderId)
        {
            if (orderId is null)
            {
                return ERROR_CODE_NULL;
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var user = await _accountService.GetUserAsync(User);

                var result = await _orderService.WarehouseManagementConfirmOrderAsync(orderId.Value, user.Id);

                if (result > 0)
                {
                    var workFlow = new WorkflowHistory()
                    {
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.Id,
                        CurrentStatus = STATUS_PREPARING_GOODS,
                        NextStatus = STATUS_WAITING_PICK_GOODS,
                        FullName = user.FullName,
                        UserEmail = user.Email,
                        RecordId = orderId.Value.ToString(),
                    };

                    var resultAddworkflow = await _workflowHistoryService.AddWorkflowHistoryAsync(workFlow);

                    if (!(resultAddworkflow is null))
                    {
                        transaction.Complete();
                        await _hubContext.Clients.Group(SIGNAL_GROUP_WAREHOUSE).SendAsync("AcceptOrders");

                        await _hubContext.Clients.Group(SIGNAL_GROUP_SHIPPER).SendAsync(SIGNAL_COUNT_ORDER_WAIT_TO_PICK);

                        return CODE_SUCCESS;
                    }
                }
            }
            catch
            {
            }

            return ERROR_CODE_SYSTEM;
        }
    }
}
