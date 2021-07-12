﻿using static Common.Constant;
using static Common.RoleConstant;
using static Common.SignalRConstant;
using Entities.Models;
using FinalProject.Heplers;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Services.Hubs;
using Microsoft.AspNetCore.SignalR;
using X.PagedList;

namespace FinalProject.Controllers
{
    [Authorize(Roles = ROLE_CUSTOMER, AuthenticationSchemes = ROLE_CUSTOMER)]
    public class OrderController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICartService _cartService;
        private readonly IConfiguration _configuration;
        private readonly IPayPalService _payPalService;
        private readonly IMoMoService _moMoService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IOrderService _orderService;
        private readonly IAddressService _addressService;
        private readonly IDeliveryAddressService _deliveryAddressService;
        private readonly IHubContext<SignalServer> _hubContext;
        private readonly IWorkflowHistoryService _workflowHistoryService;

        public OrderController(IAccountService accountService, ICartService cartService, IConfiguration configuration,
            IPayPalService payPalService, IMoMoService moMoService, IOrderDetailService orderDetailService,
            IOrderService orderService, IAddressService addressService, IDeliveryAddressService deliveryAddressService,
            IHubContext<SignalServer> hubContext, IWorkflowHistoryService workflowHistoryService)
        {
            _accountService = accountService;
            _cartService = cartService;
            _configuration = configuration;
            _payPalService = payPalService;
            _moMoService = moMoService;
            _orderDetailService = orderDetailService;
            _orderService = orderService;
            _addressService = addressService;
            _deliveryAddressService = deliveryAddressService;
            _hubContext = hubContext;
            _workflowHistoryService = workflowHistoryService;
        }
        public async Task<IActionResult> Order()
        {
            string userId = _accountService.GetUserId(User);

            ViewBag.User = await _accountService.GetUserByUserIdAsync(userId);

            var carts = await _cartService.GetCartsByUserIdAsync(userId);

            ViewBag.TotalOfCart = _cartService.GetCartTotalByUserId(userId);

            ViewBag.Provinces = await _addressService.GetProvincesAsync();

            ViewBag.Addresses = SelectHelper.ConvertDeliveryAddressesToSelectListItems(
                await _deliveryAddressService.GetDeliveryAddressesAsync(userId));

            ViewBag.Carts = carts;

            if (carts != null && carts.Any())
            {
                return View();
            }
            else
            {
                return PartialView(ERROR_1000_PAGE);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(OrderViewModel model)
        {
            try
            {
                string userId = _accountService.GetUserId(User);

                var user = await _accountService.GetUserByUserIdAsync(userId);

                var carts = await _cartService.GetCartsByUserIdAsync(userId);

                var total = _cartService.GetCartTotalByUserId(userId);

                #region ViewBag
                ViewBag.User = user;

                ViewBag.Carts = carts;

                ViewBag.TotalOfCart = total;

                ViewBag.Provinces = await _addressService.GetProvincesAsync();

                ViewBag.Addresses = SelectHelper.ConvertDeliveryAddressesToSelectListItems(
                    await _deliveryAddressService.GetDeliveryAddressesAsync(userId));
                #endregion

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                string deliveryAddressEncoder = HttpUtility.UrlEncode(model.DeliveryAddress);

                switch (model.PaymentMethod)
                {
                    case MOMO:
                        return await MoMoCheckout(total.ToString(G29), user.FullName, user.Email, deliveryAddressEncoder);
                    case PAYPAL:
                        return await PaypalCheckout(deliveryAddressEncoder);
                    case COD:
                        return await CodCheckout(model, user, total, carts);
                }

            }
            catch
            {

            }
            return View(model);
        }

        public async Task<IActionResult> Orders(string userId, int? page = 1)
        {
            if (userId is null || page is null)
            {
                return PartialView(ERROR_404_PAGE);
            }

            var result = await _orderService.GetOrdersByUserIdAsync(userId);

            var model = result.ToPagedList(page.Value, 10);

            return View(model);
        }

        public async Task<IActionResult> OrderInfo(int? orderId)
        {
            if (orderId is null)
            {
                return PartialView(ERROR_404_PAGE);
            }

            ViewBag.Order = await _orderService.GetOrderByIdAsync(orderId.Value);

            return View();
        }

        public async Task<IActionResult> CodCheckout(OrderViewModel model, User user, decimal? total, IList<CartItem> carts)
        {
            if(user is null || total is null || carts is null)
            {
                return PartialView(ERROR_404_PAGE);
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var order = await _orderService.AddOrderFromCartsAsync(carts, user, COD, model.DeliveryAddress);

            if (!(order is null))
            {
                var workFlowHistory = new WorkflowHistory()
                {
                    CurrentStatus = STATUS_CONFIRMED_PAYMENT,
                    NextStatus = STATUS_WAITING_CONFIRM,
                    CreatedBy = order.UserId,
                    CreatedDate = DateTime.Now,
                    FullName = user.FullName,
                    RecordId = order.OrderId.ToString(),
                    UserEmail = user.Email,
                    Type = TYPE_ORDER,
                    UserRole = ROLE_CUSTOMER
                };

                await _workflowHistoryService.AddWorkflowHistoryAsync(workFlowHistory);

                int result = await _orderDetailService.AddOrderDetailAsync(order, carts);

                if(result > 0)
                {
                    transaction.Complete();

                    return RedirectToAction(COD_SUCCESS, new { orderId = order.OrderId });
                }
            }

            return PartialView(ERROR_PAYMENT_PAGE);
        }

        public async Task<IActionResult> CodSuccess(int? orderId)
        {
            if (orderId is null)
            {
                return PartialView(ERROR_404_PAGE);
            }
            var user = await _accountService.GetUserAsync(User);
            await _cartService.RemoveCartItemByUserId(user.Id);
            ViewBag.OrderId = orderId.Value;

            await _hubContext.Clients.Group(SIGNAL_GROUP_ADMIN).SendAsync(SIGNAL_COUNT_NEW_ORDER);
            await _hubContext.Clients.Group(SIGNAL_GROUP_ADMIN).SendAsync(SIGNAL_TOP_PRODUCT);

            return View();
        }

        public async Task<IActionResult> PaypalCheckout(string deliveryAddress)
        {
            var userId = _accountService.GetUserId(User);
            var user = await _accountService.GetUserByUserIdAsync(userId);
            var carts = await _cartService.GetCartsByUserIdAsync(userId);
            var hostName = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            try
            {
                var createOrderResponse = await _payPalService.PayPalCreateOrder(deliveryAddress, carts, user, hostName, true);

                if (!(createOrderResponse is null))
                {
                    var createOrderResult = createOrderResponse.Result<PayPalCheckoutSdk.Orders.Order>();

                    var link = EMPTY;

                    foreach (PayPalCheckoutSdk.Orders.LinkDescription item in createOrderResult.Links)
                    {
                        if (item.Rel.Equals(PAYPAL_REL_APPROVE))
                        {
                            link = item.Href;

                            break;
                        }
                    }

                    return Redirect(link);
                }
                else throw new Exception();
            }
            catch
            {
                return PartialView(ERROR_PAYMENT_PAGE);
            }
        }

        public async Task<IActionResult> PayPalSuccess(string deliveryAddress, string token, string PayerID)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var user = await _accountService.GetUserAsync(User);

                var userInclude = await _accountService.GetUserByUserIdAsync(user.Id);

                var carts = await _cartService.GetCartsByUserIdAsync(user.Id);

                var captureOrderResponse = await _payPalService.PayPalCaptureOrder(token, true);
                string captureId = EMPTY;
                if (!(captureOrderResponse is null))
                {

                    var captureOrderResult = captureOrderResponse.Result<PayPalCheckoutSdk.Orders.Order>();

                    if(captureOrderResult.PurchaseUnits.FirstOrDefault().Payments.Captures.FirstOrDefault() != null)
                    {
                        var data = captureOrderResult.PurchaseUnits.FirstOrDefault().Payments.Captures.FirstOrDefault();
                        captureId = data.Id;
                    }

                    var order = await _orderService.AddOrderFromCartsAsync(carts, userInclude, PAYPAL, HttpUtility.UrlDecode(deliveryAddress));

                    if (!(order is null))
                    {
                        var workFlowHistory = new WorkflowHistory()
                        {
                            CurrentStatus = STATUS_CONFIRMED_PAYMENT,
                            NextStatus = STATUS_WAITING_CONFIRM,
                            CreatedBy = order.UserId,
                            CreatedDate = DateTime.Now,
                            FullName = user.FullName,
                            RecordId = order.OrderId.ToString(),
                            UserEmail = user.Email,
                            Type = TYPE_ORDER,
                            UserRole = ROLE_CUSTOMER
                        };

                        await _workflowHistoryService.AddWorkflowHistoryAsync(workFlowHistory);

                        int result = await _orderDetailService.AddOrderDetailAsync(order, carts);

                        if (result > 0)
                        {
                            result = await _payPalService
                                .AddPayPalPaymentAsync(order.OrderId, token, PayerID, captureOrderResult.Links.FirstOrDefault().Href, captureId);

                            if (result > 0)
                            {
                                await _cartService.RemoveCartItemByUserId(user.Id);

                                transaction.Complete();

                                ViewBag.OrderId = order.OrderId;

                                await _hubContext.Clients.Group(SIGNAL_GROUP_ADMIN).SendAsync(SIGNAL_COUNT_NEW_ORDER);
                                await _hubContext.Clients.Group(SIGNAL_GROUP_ADMIN).SendAsync(SIGNAL_TOP_PRODUCT);
                            }
                        }
                    }

                    return View();
                }

                else throw new Exception();
            }
            catch
            {
                return PartialView(ERROR_PAYMENT_PAGE);
            }
        }

        public async Task<IActionResult> MoMoCheckout(string total, string orderInfo, string email, string deliveryAddress)
        {
            string hostName = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            try
            {
                string responseFromMomo = await _moMoService.MoMoCheckoutAsync(total, orderInfo, email, hostName, deliveryAddress);

                JObject jmessage = JObject.Parse(responseFromMomo);

                string redirect = jmessage.GetValue(PAYPAL_URL).ToString();

                return Redirect(redirect);
            }
            catch
            {
                return PartialView(ERROR_PAYMENT_PAGE);
            }
        }

        public async Task<IActionResult> MoMoSuccess(string deliveryAddress, string orderId, string payType, string responseTime, string errorCode, string amount, string transId)
        {
            if (errorCode.Equals(ZERO))
            {
                var user = await _accountService.GetUserAsync(User);

                var userInclude = await _accountService.GetUserByUserIdAsync(user.Id);

                var carts = await _cartService.GetCartsByUserIdAsync(user.Id);

                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                try
                {
                    var order = await _orderService.AddOrderFromCartsAsync(carts, userInclude, MOMO, HttpUtility.UrlDecode(deliveryAddress));

                    if (!(order is null))
                    {
                        var workFlowHistory = new WorkflowHistory()
                        {
                            CurrentStatus = STATUS_CONFIRMED_PAYMENT,
                            NextStatus = STATUS_WAITING_CONFIRM,
                            CreatedBy = order.UserId,
                            CreatedDate = DateTime.Now,
                            FullName = user.FullName,
                            RecordId = order.OrderId.ToString(),
                            UserEmail = user.Email,
                            Type = TYPE_ORDER,
                            UserRole = ROLE_CUSTOMER
                        };

                        await _workflowHistoryService.AddWorkflowHistoryAsync(workFlowHistory);

                        int result = await _orderDetailService.AddOrderDetailAsync(order, carts);

                        if (result > 0)
                        {
                            result = await _moMoService.AddMoMoPaymentAsync(order.OrderId, orderId, payType, responseTime, amount, transId);

                            if (result > 0)
                            {
                                await _cartService.RemoveCartItemByUserId(user.Id);

                                transaction.Complete();

                                ViewBag.OrderId = order.OrderId;

                                await _hubContext.Clients.Group(SIGNAL_GROUP_ADMIN).SendAsync(SIGNAL_COUNT_NEW_ORDER);
                                await _hubContext.Clients.Group(SIGNAL_GROUP_ADMIN).SendAsync(SIGNAL_TOP_PRODUCT);

                                return View();
                            }
                        }
                    }
                }
                catch
                {

                }
            }

            return PartialView(ERROR_PAYMENT_PAGE);
        }

        public async Task<int> RejectOrder(int? orderId, string content)
        {
            if(orderId is null || content is null)
            {
                return ERROR_CODE_NULL;
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId.Value);

                if (order != null)
                {
                    var user = await _accountService.GetUserAsync(User);

                    order.Note = content;
                    order.ModifiedBy = user.Id;
                    order.ModifiedDate = DateTime.Now;
                    order.Status = STATUS_PENDING_ADMIN_CANCED_ORDER;

                    var result = await _orderService.UpdateOrderAsync(order);

                    if (result > 0)
                    {
                        var workflow = new WorkflowHistory()
                        {
                            CurrentStatus = STATUS_WAITING_CONFIRM,
                            NextStatus = STATUS_PENDING_ADMIN_CANCED_ORDER,
                            CreatedBy = order.UserId,
                            CreatedDate = DateTime.Now,
                            FullName = user.FullName,
                            RecordId = order.OrderId.ToString(),
                            UserEmail = user.Email,
                            Type = TYPE_ORDER,
                            UserRole = ROLE_CUSTOMER
                        };

                        var workflowHistory = await _workflowHistoryService.AddWorkflowHistoryAsync(workflow);

                        if (workflowHistory != null)
                        {
                            transaction.Complete();

                            await _hubContext.Clients.Group(SIGNAL_GROUP_ADMIN).SendAsync(SIGNAL_COUNT_ORDER_WAIT_TO_REJECT);

                            return CODE_SUCCESS;
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
