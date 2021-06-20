using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Hubs;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using static Common.Constant;
using static Common.RoleConstant;
using static Common.SignalRConstant;

namespace FinalProject.Areas.Shipper.Controllers
{
    [Area(AREA_SHIPPER)]
    [Authorize(Roles = ROLE_SHIPPER, AuthenticationSchemes = ROLE_SHIPPER)]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;
        private readonly IWorkflowHistoryService _workflowHistoryService;
        private readonly IHubContext<SignalServer> _hubContext;

        public OrderController(IOrderService orderService, IWorkflowHistoryService workflowHistoryService, IAccountService accountService, IHubContext<SignalServer> hubContext)
        {
            _orderService = orderService;
            _accountService = accountService;
            _workflowHistoryService = workflowHistoryService;
            _hubContext = hubContext;
        }
        public async Task<IActionResult> OrderWaitPick(string customer = EMPTY, string orderDate = EMPTY, string address = EMPTY)
        {
            ViewBag.Addresses = await _orderService.GetAddressinOrdersAsync();
            ViewBag.Customers = await _accountService.GetAllCustomersAsync();

            if (customer != EMPTY || orderDate !=  EMPTY || address != EMPTY)
            {
                ViewBag.Orders = _orderService.GetOrdersWaitToPick(customer, orderDate, address);
            }
            else
            {
                ViewBag.Orders = _orderService.GetOrdersWaitToPick();
            }

            return View();
        }

        public async Task<IActionResult> OrderHistory(int? orderId)
        {
            if(orderId is null)
            {
                return PartialView(ERROR_404_PAGE_SHIPPER);
            }

            ViewBag.OrderHistory = await _orderService.GetOrderHistoryAsync(orderId.Value);

            return View();
        }

        public async Task<IActionResult> OrderDelivered(string receivedDate = EMPTY, string customer = EMPTY, string shipper = EMPTY)
        {
            var user = await _accountService.GetUserAsync(User);
            ViewBag.Customers = await _accountService.GetAllCustomersAsync();

            if (receivedDate != null && !receivedDate.Equals(EMPTY) || !customer.Equals(EMPTY))
            {
                ViewBag.Orders = _orderService.GetDateTimeDeliveredByFilter(customer, shipper, receivedDate, user.Id);
            }
            else
            {
                ViewBag.Orders = _orderService.GetOrderDelivered(user.Id);
            }

            return View();
        }

        [HttpPut]
        public async Task<string> ConfirmOrder(IList<int> ordersPicked, IList<string> rowVersion)
        {
            if(ordersPicked is null)
            {
                return ERROR_CODE_NULL.ToString();
            }
            try
            {
                var rowVersionBytes = new List<byte[]>();
                foreach(var item in rowVersion)
                {
                    rowVersionBytes.Add(Convert.FromBase64String(item));
                }

                var user = await _accountService.GetUserAsync(User);
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var result = await _orderService.ShipperConfirmPickOrdersAsync(ordersPicked, user.Id, rowVersionBytes);

                var jsonConvert = JsonConvert.DeserializeObject<JObject>(result);

                if (jsonConvert.Count > 0)
                {
                    transaction.Complete();

                    await _hubContext.Clients.Group(SIGNAL_GROUP_SHIPPER).SendAsync(SIGNAL_COUNT_ORDER_WAIT_TO_PICK);
                    await _hubContext.Clients.User(user.Id).SendAsync(SIGNAL_COUNT_ORDER_WAIT_TO_DELIVERY);

                    return result;
                }
            }
            catch
            {
            }

            return ERROR_CODE_SYSTEM.ToString();
        }

        public async Task<IActionResult> OrderWaitDelivery(string customer = EMPTY, string pickedOrderDate = EMPTY, string address = EMPTY)
        {
            ViewBag.Customers = await _accountService.GetAllCustomersAsync();
            ViewBag.Addresses = await _orderService.GetAddressinOrdersAsync();
            var user = await _accountService.GetUserAsync(User);
            if (customer != EMPTY || pickedOrderDate != EMPTY || address != EMPTY)
            {
                ViewBag.Orders = await _orderService.GetOrdersWaitDeliveryAsync(user.Id, customer, pickedOrderDate, address);
            }
            else
            {
                ViewBag.Orders = await _orderService.GetOrdersWaitDeliveryAsync(user.Id);
            }

            return View();
        }

        public async Task<IActionResult> OrderDelivery(string exportWarehouseDate = EMPTY, string receivedDeliveryDate = EMPTY, string customer = EMPTY, string shipper = EMPTY)
        {
            var user = await _accountService.GetUserAsync(User);
            ViewBag.Customers = await _accountService.GetAllCustomersAsync();
            ViewBag.Warehouses = await _accountService.GetAllWarehouseManagementsAsync();

            if (exportWarehouseDate != null || receivedDeliveryDate != null || !customer.Equals(EMPTY))
            {
                ViewBag.Orders = _orderService.FilterOrder(exportWarehouseDate, receivedDeliveryDate, EMPTY, shipper, user.Id, customer);
            }
            else
            {
                ViewBag.Orders = _orderService.GetOrdersWaitDelivery();
            }
            return View();
        }

        public async Task<IActionResult> OrderDetail(int? orderId)
        {
            if (orderId is null)
            {
                return PartialView(ERROR_404_PAGE_SHIPPER);
            }

            ViewBag.Order = await _orderService.GetOrderByIdAsync(orderId.Value);

            ViewBag.User = await _accountService.GetUserAsync(User);

            return View();
        }

        [HttpPut]
        public async Task<int> ShipperComfirmOrder(int? orderId)
        {
            if (orderId is null)
            {
                return ERROR_CODE_NULL;
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var user = await _accountService.GetUserAsync(User);

                var result = await _orderService.ShipperConfirmOrderAsync(orderId.Value);

                if (result > 0)
                {
                    var workFlow = new WorkflowHistory()
                    {
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.Id,
                        CurrentStatus = STATUS_WAITING_PICK_GOODS,
                        NextStatus = STATUS_ON_DELIVERY_GOODS,
                        FullName = user.FullName,
                        UserEmail = user.Email,
                        RecordId = orderId.Value.ToString(),
                        Type = TYPE_ORDER,
                        UserRole = ROLE_SHIPPER
                    };

                    var resultAddworkflow = await _workflowHistoryService.AddWorkflowHistoryAsync(workFlow);

                    if (!(resultAddworkflow is null))
                    {
                        transaction.Complete();

                        await _hubContext.Clients.Group(SIGNAL_GROUP_SHIPPER).SendAsync(SIGNAL_COUNT_ORDER_WAIT_TO_PICK);
                        await _hubContext.Clients.User(user.Id).SendAsync(SIGNAL_COUNT_ORDER_WAIT_TO_DELIVERY);
                        await _hubContext.Clients.User(user.Id).SendAsync(SIGNAL_COUNT_ORDER_DELIVERING);

                        return CODE_SUCCESS;
                    }
                }
            }
            catch
            {
            }

            return ERROR_CODE_SYSTEM;
        }

        [HttpPut]
        public async Task<int> ShipperComfirmDelivery(int? orderId)
        {
            if (orderId is null)
            {
                return ERROR_CODE_NULL;
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var user = await _accountService.GetUserAsync(User);

                var result = await _orderService.ShipperConfirmDeliveryAsync(orderId.Value);

                if (result > 0)
                {
                    var workFlow = new WorkflowHistory()
                    {
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.Id,
                        CurrentStatus = STATUS_ON_DELIVERY_GOODS,
                        NextStatus = STATUS_RECEIVED_GOODS,
                        FullName = user.FullName,
                        UserEmail = user.Email,
                        RecordId = orderId.Value.ToString(),
                        Type = TYPE_ORDER,
                        UserRole = ROLE_SHIPPER
                    };

                    var resultAddworkflow = await _workflowHistoryService.AddWorkflowHistoryAsync(workFlow);

                    if (!(resultAddworkflow is null))
                    {
                        transaction.Complete();

                        await _hubContext.Clients.User(user.Id).SendAsync(SIGNAL_COUNT_ORDER_DELIVERING);

                        await _hubContext.Clients.User(user.Id).SendAsync(SIGNAL_COUNT_ORDER_DELIVERED);

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
