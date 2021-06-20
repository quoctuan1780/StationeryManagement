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

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
    [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
    public class OrderController : Controller
    {
        private readonly IHubContext<SignalServer> _hubContext;
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;
        private readonly IWorkflowHistoryService _workflowHistoryService;

        public OrderController(IOrderService orderService, IAccountService accountService, IWorkflowHistoryService workflowHistoryService, IHubContext<SignalServer> hubContext)
        {
            _hubContext = hubContext;
            _orderService = orderService;
            _accountService = accountService;
            _workflowHistoryService = workflowHistoryService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Orders = await _orderService.GetOrdersAsync();
            return View();
        }

        public async Task<IActionResult> OrderDetail(int? orderId)
        {
            if (orderId is null)
            {
                return PartialView(ERROR_404_PAGE_ADMIN);
            }

            ViewBag.Order = await _orderService.GetOrderByIdAsync(orderId.Value);

            return View();
        }

        public async Task<IActionResult> OrderHistory(int? orderId)
        {
            if (orderId is null)
            {
                return PartialView(ERROR_404_PAGE_SHIPPER);
            }

            ViewBag.OrderHistory = await _orderService.GetOrderHistoryAsync(orderId.Value);

            return View();
        }

        public async Task<IActionResult> OrderWaitComfirm(string orderDate = EMPTY, string paymentMethod = EMPTY, string customer = EMPTY)
        {
            ViewBag.Customers = await _accountService.GetAllCustomersAsync();
            ViewBag.Orders = await _orderService.GetOrdersWaitToConfirmAsync(orderDate, paymentMethod, customer);

            return View();
        }

        public async Task<IActionResult> OrderWaitExportWarehouse(string customer = EMPTY)
        {
            ViewBag.Customers = await _accountService.GetAllCustomersAsync();
            ViewBag.Orders = await _orderService.GetOrdersWaitExportWarehouseAsync(customer);

            return View();
        }

        public async Task<IActionResult> OrderDelivery(string exportWarehouseDate = EMPTY, string receivedDeliveryDate = EMPTY, string warehouse = EMPTY, string shipper = EMPTY)
        {
            ViewBag.Shippers = await _accountService.GetAllShippersAsync();
            ViewBag.Warehouses = await _accountService.GetAllWarehouseManagementsAsync();

            if (exportWarehouseDate != null || receivedDeliveryDate != null || !shipper.Equals(EMPTY) || !warehouse.Equals(EMPTY))
            {
                ViewBag.Orders = _orderService.FilterOrder(exportWarehouseDate, receivedDeliveryDate, warehouse, shipper);
            }
            else
            {
                ViewBag.Orders = _orderService.GetOrdersWaitDelivery();
            }
            return View();
        }

        public async Task<IActionResult> OrderWaitToPick(string exportWarehouseDate = EMPTY , string warehouse = EMPTY, string customer = EMPTY)
        {
            ViewBag.Warehouses = await _accountService.GetAllWarehouseManagementsAsync();
            ViewBag.Customers = await _accountService.GetAllCustomersAsync();

            if (exportWarehouseDate != null  || !customer.Equals(EMPTY) || !warehouse.Equals(EMPTY))
            {
                ViewBag.Orders = _orderService.FilterOrder(exportWarehouseDate, warehouse, customer);
            }
            else
            {
                ViewBag.Orders = _orderService.GetOrdersWaitToPick();
            }

            return View();
        }

        public async Task<IActionResult> OrderDelivered(string receivedDate = EMPTY, string customer = EMPTY, string shipper = EMPTY)
        {
            ViewBag.Shippers = await _accountService.GetAllShippersAsync();
            ViewBag.Customers = await _accountService.GetAllCustomersAsync();
            ViewBag.ReceivedDates = _orderService.GetDateTimeDelivered();

            if (!receivedDate.Equals(EMPTY) || !customer.Equals(EMPTY) || !shipper.Equals(EMPTY))
            {
                ViewBag.Orders = _orderService.GetDateTimeDeliveredByFilter(customer, shipper, receivedDate);
            }
            else
            {
                ViewBag.Orders = _orderService.GetOrderDelivered();
            }

            return View();
        }

        [HttpPut]
        public async Task<int> AdminComfirmOrder(int? orderId)
        {
            if (orderId is null)
            {
                return ERROR_CODE_NULL;
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var user = await _accountService.GetUserAsync(User);

                var result = await _orderService.AdminConfirmOrderAsync(orderId.Value);

                if (result > 0)
                {
                    var workFlow = new WorkflowHistory()
                    {
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.Id,
                        CurrentStatus = STATUS_WAITING_CONFIRM,
                        NextStatus = STATUS_PREPARING_GOODS,
                        FullName = user.FullName,
                        UserEmail = user.Email,
                        RecordId = orderId.Value.ToString(),
                        Type = TYPE_ORDER,
                        UserRole = ROLE_ADMIN
                    };

                    var resultAddworkflow = await _workflowHistoryService.AddWorkflowHistoryAsync(workFlow);

                    if (!(resultAddworkflow is null))
                    {
                        transaction.Complete();

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
