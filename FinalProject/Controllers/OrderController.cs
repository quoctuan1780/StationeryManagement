﻿using Common;
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

namespace FinalProject.Controllers
{
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

        public OrderController(IAccountService accountService, ICartService cartService, IConfiguration configuration,
            IPayPalService payPalService, IMoMoService moMoService, IOrderDetailService orderDetailService,
            IOrderService orderService, IAddressService addressService, IDeliveryAddressService deliveryAddressService)
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
        }
        public async Task<IActionResult> Order()
        {
            try
            {
                string userId = _accountService.GetUserId(User);

                ViewBag.User = await _accountService.GetUserByUserIdAsync(userId);

                ViewBag.Carts = await _cartService.GetCartsByUserIdAsync(userId);

                ViewBag.TotalOfCart = _cartService.GetCartTotalByUserId(userId);

                ViewBag.Provinces = await _addressService.GetProvincesAsync();

                ViewBag.Addresses = SelectHelper.ConvertDeliveryAddressesToSelectListItems(
                    await _deliveryAddressService.GetDeliveryAddressesAsync(userId));

            }
            catch
            {

            }

            return View();


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
                    case Constant.MOMO:
                        return MoMoCheckout(total.ToString(Constant.G29), user.FullName, user.Email, deliveryAddressEncoder);
                    case Constant.PAYPAL:
                        return await PaypalCheckout(deliveryAddressEncoder);
                    case Constant.COD:
                        return await CodCheckout(model, user, total, carts);
                }

            }
            catch
            {

            }
            return View(model);
        }

        public async Task<IActionResult> Orders(string userId)
        {
            if (userId is null)
            {
                return PartialView(Constant.ERROR_404_PAGE);
            }

            try
            {
                ViewBag.Orders = await _orderService.GetOrdersByUserIdAsync(userId);
            }
            catch
            {
                return PartialView(Constant.ERROR_404_PAGE);
            }

            return View();
        }

        public async Task<IActionResult> OrderInfo(int? orderId)
        {
            if (orderId is null)
            {
                return PartialView(Constant.ERROR_404_PAGE);
            }
            try
            {
                ViewBag.Order = await _orderService.GetOrderByIdAsync(orderId.Value);
            }
            catch
            {
                return PartialView(Constant.ERROR_404_PAGE);
            }

            return View();
        }

        public async Task<IActionResult> CodCheckout(OrderViewModel model, User user, decimal? total, IList<CartItem> carts)
        {
            if(user is null || total is null || carts is null)
            {
                return PartialView(Constant.ERROR_404_PAGE);
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var order = await _orderService.AddOrderFromCartsAsync(carts, user, Constant.COD, model.DeliveryAddress);

            if (!(order is null))
            {
                int result = await _orderDetailService.AddOrderDetailAsync(order, carts);

                if(result > 0)
                {
                    transaction.Complete();

                    return RedirectToAction("CodSuccess", new { orderId = order.OrderId });
                }
            }

            return Redirect("/Home/Error");
        }

        public IActionResult CodSuccess(int? orderId)
        {
            if (orderId is null)
            {
                return PartialView(Constant.ERROR_404_PAGE);
            }

            ViewBag.OrderId = orderId.Value;

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

                    var link = Constant.EMPTY;

                    foreach (PayPalCheckoutSdk.Orders.LinkDescription item in createOrderResult.Links)
                    {
                        if (item.Rel.Equals(Constant.PAYPAL_REL_APPROVE))
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
                return PartialView(Constant.ERROR_PAYMENT_PAGE);
            }
        }

        public IActionResult MoMoCheckout(string total, string orderInfo, string email, string deliveryAddress)
        {
            string hostName = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            try
            {
                string responseFromMomo = _moMoService.MoMoCheckout(total, orderInfo, email, hostName, deliveryAddress);

                JObject jmessage = JObject.Parse(responseFromMomo);

                string redirect = jmessage.GetValue(Constant.PAYPAL_URL).ToString();

                return Redirect(redirect);
            }
            catch
            {
                return PartialView(Constant.ERROR_PAYMENT_PAGE);
            }
        }

        public async Task<IActionResult> MoMoSuccess(string deliveryAddress, string orderId, string payType, string responseTime, string errorCode)
        {
            if (errorCode.Equals(Constant.ZERO))
            {
                var user = await _accountService.GetUserAsync(User);

                var userInclude = await _accountService.GetUserByUserIdAsync(user.Id);

                var carts = await _cartService.GetCartsByUserIdAsync(user.Id);

                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                try
                {
                    var order = await _orderService.AddOrderFromCartsAsync(carts, userInclude, Constant.MOMO, HttpUtility.UrlDecode(deliveryAddress));

                    if (!(order is null))
                    {
                        int result = await _orderDetailService.AddOrderDetailAsync(order, carts);

                        if (result > 0)
                        {
                            result = await _moMoService.AddMoMoPaymentAsync(order.OrderId, orderId, payType, responseTime);

                            if (result > 0)
                            {
                                transaction.Complete();

                                ViewBag.OrderId = order.OrderId;

                                return View();
                            }
                        }
                    }
                }
                catch
                {

                }
            }

            return PartialView(Constant.ERROR_PAYMENT_PAGE);
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

                if (!(captureOrderResponse is null))
                {

                    var captureOrderResult = captureOrderResponse.Result<PayPalCheckoutSdk.Orders.Order>();

                    var order = await _orderService.AddOrderFromCartsAsync(carts, userInclude, Constant.PAYPAL, HttpUtility.UrlDecode(deliveryAddress));

                    if (!(order is null))
                    {
                        int result = await _orderDetailService.AddOrderDetailAsync(order, carts);

                        if (result > 0)
                        {
                            result = await _payPalService
                                .AddPayPalPaymentAsync(order.OrderId, token, PayerID, captureOrderResult.Links.FirstOrDefault().Href);

                            if (result > 0)
                            {
                                transaction.Complete();

                                ViewBag.OrderId = order.OrderId;
                            }
                        }
                    }

                    return View();
                }

                else throw new Exception();
            }
            catch
            {
                return Redirect("/Home/Error");
            }
        }

        //public async Task<string> GetDeliveryFeeShip()
        //{
        //    return await _fastDeliveryService.CaculateFeeShipAsync();
        //}
    }
}
