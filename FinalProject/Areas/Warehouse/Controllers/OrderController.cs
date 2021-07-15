using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Services.Hubs;
using Services.Interfacies;
using System;
using System.Collections.Generic;
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
        private readonly IBillService _billService;
        private IWorkflowHistoryService _workflowHistoryService;
        private INotificationService _notificationService;

        public OrderController(IOrderService orderService, IWorkflowHistoryService workflowHistoryService, IAccountService accountService, IHubContext<SignalServer> hubContext, IBillService billService, INotificationService notificationService)
        {
            _hubContext = hubContext;
            _orderService = orderService;
            _accountService = accountService;
            _billService = billService;
            _workflowHistoryService = workflowHistoryService;
            _notificationService = notificationService;
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

        [HttpPut]
        public async Task<int> PrepareOrder(int? OrderId, int? ProductId)
        {
            if(OrderId is null || ProductId is null)
            {
                return ERROR_CODE_NULL;
            }

            var result = await _orderService.PrepareOrder(OrderId.Value, ProductId.Value);

            if(result > 0)
            {
                return CODE_SUCCESS;
            }

            return ERROR_CODE_SYSTEM;
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

                    var resultAddBill = await _billService.AddBillWithOrderIdAsync(orderId.Value);

                    if (!(resultAddworkflow is null))
                    {
                        var link = Url.ActionLink("OrderWaitPick", "Order",
                                            new { Area = AREA_SHIPPER },
                                            Request.Scheme);

                        var notificationType = await _notificationService.GetNotifycationByNameAsync(NOTIFICATION_DELIVERY_ORDER);

                        var notifications = new List<Notification>();

                        var shippers = await _accountService.GetAllShippersAsync();

                        var content = "Đơn hàng #" + orderId.Value + " đã chuẩn bị xong";
                        if (shippers != null)
                        {
                            foreach (var item in shippers)
                            {
                                var nofification = new Notification()
                                {
                                    CreatedDate = DateTime.Now,
                                    Link = link,
                                    NotificationTypeId = notificationType.NotificationTypeId,
                                    Status = STATUS_NOT_SEEN_NOTIFICATION,
                                    UserId = item.Id,
                                    RecordId = orderId.Value,
                                    RoleSeen = ROLE_SHIPPER,
                                    Content = content
                                };

                                notifications.Add(nofification);
                            }
                        }

                        await _notificationService.AddNotificationAsync(notifications);

                        transaction.Complete();

                        await _hubContext.Clients.Group(SIGNAL_GROUP_SHIPPER).SendAsync(SIGNAL_NOTIFICATION_NEW_ORDER_DELIVERY, link, DateTime.Now.ToShortDateString(), content);
                        await _hubContext.Clients.Group(SIGNAL_GROUP_ADMIN).SendAsync(SIGNAL_TOP_REVENUE);
                        await _hubContext.Clients.Group(SIGNAL_GROUP_ADMIN).SendAsync(SIGNAL_TOP_PRODUCT);
                        await _hubContext.Clients.Group(SIGNAL_GROUP_ADMIN).SendAsync(SIGNAL_TOP_REVENUE_CURRENT_MONTH);
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


        #region Feature Developer
        //public async Task<int> PrepareOrder(int OrderId, int ProductId)
        //{
        //    return await _orderService.PrepareOrder(OrderId,ProductId);
        //}
        //public async Task<int> RejectOrder(int id)
        //{
        //    return await _orderService.RejectOrder(id);
        //}
        #endregion
    }
}
