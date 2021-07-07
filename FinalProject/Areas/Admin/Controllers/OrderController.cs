using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private readonly IMoMoService _moMoService;
        private readonly IPayPalService _payPalService;
        private readonly IWorkflowHistoryService _workflowHistoryService;

        public OrderController(IOrderService orderService, IAccountService accountService, IWorkflowHistoryService workflowHistoryService, IHubContext<SignalServer> hubContext, IMoMoService moMoService, IPayPalService payPalService)
        {
            _hubContext = hubContext;
            _orderService = orderService;
            _accountService = accountService;
            _moMoService = moMoService;
            _payPalService = payPalService;
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
                        await _hubContext.Clients.Group(SIGNAL_GROUP_WAREHOUSE).SendAsync("AcceptOrders");

                        return CODE_SUCCESS;
                    }
                }
            }
            catch
            {
            }

            return ERROR_CODE_SYSTEM;
        }

        public async Task<IActionResult> OrderWaitReject(string customer = EMPTY, string paymentMethod = EMPTY)
        {
            ViewBag.Customers = await _accountService.GetAllCustomersAsync();

            if (!customer.Equals(EMPTY) && !paymentMethod.Equals(EMPTY))
            {
                ViewBag.Orders =  _orderService.FilterOrder(customer, paymentMethod);
            }
            else
            {
                ViewBag.Orders = _orderService.FilterOrder();
            }

            return View();
        }

        [HttpPut]
        public async Task<int> RejectOrder(int? orderId, string content = EMPTY)
        {
            if(orderId is null)
            {
                return ERROR_CODE_NULL;
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId.Value);
                var user = await _accountService.GetUserAsync(User);
                if(order != null)
                {
                    order.Status = STATUS_CANCELED;
                    order.ModifiedBy = user.Id;
                    order.ModifiedDate = DateTime.Now;
                    if(!content.Equals(EMPTY))
                    {
                        order.Note = content;
                    }

                    var orderUpdate = await _orderService.UpdateOrderAsync(order);
                    if (orderUpdate > 0)
                    {
                        var workflow = new WorkflowHistory()
                        {
                            CreatedDate = DateTime.Now,
                            CreatedBy = user.Id,
                            FullName = user.FullName,
                            UserEmail = user.Email,
                            RecordId = orderId.Value.ToString(),
                            Type = TYPE_ORDER,
                            UserRole = ROLE_ADMIN
                        };

                        if (content.Equals(EMPTY))
                        {
                            workflow.CurrentStatus = STATUS_WAITING_CONFIRM;
                            workflow.NextStatus = STATUS_CANCELED;
                        }
                        else
                        {
                            workflow.CurrentStatus = STATUS_PENDING_ADMIN_CANCED_ORDER;
                            workflow.NextStatus = STATUS_CANCELED;
                        }

                        var workflowAdd = await _workflowHistoryService.AddWorkflowHistoryAsync(workflow);

                        if (workflowAdd != null)
                        {
                            switch (order.PaymentMethod)
                            {
                                case MOMO:
                                    var result = await _moMoService.RefundMoneyAsync(order.MoMoPayment.MoMoOrderId, order.MoMoPayment.TransId, order.MoMoPayment.Amount);

                                    var json = JsonConvert.DeserializeObject<JObject>(result);

                                    if (json.Value<int>("status") == 0)
                                    {
                                        transaction.Complete();

                                        return CODE_SUCCESS;
                                    }

                                    return CODE_FAIL;
                                case PAYPAL:
                                    var total = Math.Round((order.Total / (decimal)EXCHANGE_RATE_USD), 2).ToString().Replace(COMMA, DOT);

                                    var resultPaypal  = await _payPalService.CapturesRefund(order.PayPalPayment.CaptureId, total);

                                    if (resultPaypal != null)
                                    {
                                        var captureRefundResult = resultPaypal.Result<PayPalCheckoutSdk.Payments.Refund>();

                                        transaction.Complete();

                                        return CODE_SUCCESS;
                                    }
                                    return CODE_FAIL;
                                case COD:
                                    transaction.Complete();

                                    return CODE_SUCCESS;
                            }
                        }
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
