using Common;
using FinalProject.Heplers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

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

        public OrderController(IAccountService accountService, ICartService cartService, IConfiguration configuration,
            IPayPalService payPalService, IMoMoService moMoService, IOrderDetailService orderDetailService,
            IOrderService orderService)
        {
            _accountService = accountService;
            _cartService = cartService;
            _configuration = configuration;
            _payPalService = payPalService;
            _moMoService = moMoService;
            _orderDetailService = orderDetailService;
            _orderService = orderService;
        }
        public async Task<IActionResult> Order()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    ViewBag.User = await _accountService.GetUserAsync(User);

            //    return View();
            //}
            //else
            //    return Redirect(Constant.ROUTE_LOGIN_CLIENT);

            var userId = _accountService.GetUserId(User);

            ViewBag.User = await _accountService.GetUserByUserIdAsync(userId);

            ViewBag.Carts = await _cartService.GetCartsByUserIdAsync(userId);

            ViewBag.TotalOfCart = _cartService.GetCartTotalByUserId(userId);

            return View();
        }

        public async Task<IActionResult> PaypalCheckout()
        {
            var userId = _accountService.GetUserId(User);
            var user = await _accountService.GetUserByUserIdAsync(userId);
            var carts = await _cartService.GetCartsByUserIdAsync(userId);
            var hostName = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            try
            {
                var createOrderResponse = await _payPalService.PayPalCreateOrder(carts, user, hostName, true);

                if (!(createOrderResponse is null))
                {
                    var createOrderResult = createOrderResponse.Result<PayPalCheckoutSdk.Orders.Order>();

                    var link = Constant.EMPTY;

                    foreach (var item in createOrderResult.Links)
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
                return Redirect("/Home/Error");
            }
        }

        public IActionResult MoMoCheckout(string total, string orderInfo, string email)
        {
            var hostName = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            try
            {
                var responseFromMomo = _moMoService.MoMoCheckout(total, orderInfo, email, hostName);

                var jmessage = JObject.Parse(responseFromMomo);

                var redirect = jmessage.GetValue("payUrl").ToString();

                return Redirect(redirect);
            }
            catch
            {
                return Redirect("/Home/Error");
            }
        }

        public async Task<IActionResult> MoMoSuccess(string orderId, string payType, string responseTime, string errorCode)
        {
            if (errorCode.Equals(Constant.ZERO))
            {
                var user = await _accountService.GetUserAsync(User);

                var userInclude = await _accountService.GetUserByUserIdAsync(user.Id);

                var carts = await _cartService.GetCartsByUserIdAsync(user.Id);

                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                try
                {
                    var order = await _orderService.AddOrderFromCartsAsync(carts, userInclude, "MoMo");

                    if (!(order is null))
                    {
                        var result = await _orderDetailService.AddOrderDetailAsync(order, carts);

                        if (result > 0)
                        {
                            result = await _moMoService.AddMoMoPaymentAsync(order.OrderId, orderId, payType, responseTime);

                            if (result > 0)
                            {
                                transaction.Complete();
                            }
                        }
                    }
                }
                catch
                {

                }

                return View();
            }

            return Redirect("/Home/Error");
        }

        public async Task<IActionResult> PayPalSuccess(string token, string PayerID)
        {
            var user = await _accountService.GetUserAsync(User);

            var userInclude = await _accountService.GetUserByUserIdAsync(user.Id);

            var carts = await _cartService.GetCartsByUserIdAsync(user.Id);

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var captureOrderResponse = await _payPalService.PayPalCaptureOrder(token, true);

                if (!(captureOrderResponse is null))
                {

                    var captureOrderResult = captureOrderResponse.Result<PayPalCheckoutSdk.Orders.Order>();

                    var order = await _orderService.AddOrderFromCartsAsync(carts, userInclude, "PayPal");

                    if (!(order is null))
                    {
                        var result = await _orderDetailService.AddOrderDetailAsync(order, carts);

                        if (result > 0)
                        {
                            result = await _payPalService
                                .AddPayPalPaymentAsync(order.OrderId, token, PayerID, captureOrderResult.Links.FirstOrDefault().Href);

                            if (result > 0)
                            {
                                transaction.Complete();
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
    }
}
