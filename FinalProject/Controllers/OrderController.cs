using Common;
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

            string userId = _accountService.GetUserId(User);

            ViewBag.User = await _accountService.GetUserByUserIdAsync(userId);

            ViewBag.Carts = await _cartService.GetCartsByUserIdAsync(userId);

            ViewBag.TotalOfCart = _cartService.GetCartTotalByUserId(userId);

            return View();
        }

        public async Task<IActionResult> Orders(string userId)
        {
            if(userId is null)
            {
                //return Redirect("/Home/NotFound");
                return PartialView(Constant.ERROR_404_PAGE);
            }

            ViewBag.Orders = await _orderService.GetOrdersByUserIdAsync(userId);

            return View();
        }

        public async Task<IActionResult> OrderInfo(int? orderId)
        {
            if (orderId is null)
            {
                return BadRequest();
            }

            ViewBag.Order = await _orderService.GetOrderByIdAsync(orderId.Value);

            return View();
        }

        public async Task<IActionResult> PaypalCheckout()
        {
            string userId = _accountService.GetUserId(User);
            Entities.Models.User user = await _accountService.GetUserByUserIdAsync(userId);
            IList<Entities.Models.CartItem> carts = await _cartService.GetCartsByUserIdAsync(userId);
            string hostName = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            try
            {
                PayPalHttp.HttpResponse createOrderResponse = await _payPalService.PayPalCreateOrder(carts, user, hostName, true);

                if (!(createOrderResponse is null))
                {
                    PayPalCheckoutSdk.Orders.Order createOrderResult = createOrderResponse.Result<PayPalCheckoutSdk.Orders.Order>();

                    string link = Constant.EMPTY;

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
                return Redirect("/Home/Error");
            }
        }

        public IActionResult MoMoCheckout(string total, string orderInfo, string email)
        {
            string hostName = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            try
            {
                string responseFromMomo = _moMoService.MoMoCheckout(total, orderInfo, email, hostName);

                JObject jmessage = JObject.Parse(responseFromMomo);

                string redirect = jmessage.GetValue("payUrl").ToString();

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
                Entities.Models.User user = await _accountService.GetUserAsync(User);

                Entities.Models.User userInclude = await _accountService.GetUserByUserIdAsync(user.Id);

                IList<Entities.Models.CartItem> carts = await _cartService.GetCartsByUserIdAsync(user.Id);

                using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                try
                {
                    Entities.Models.Order order = await _orderService.AddOrderFromCartsAsync(carts, userInclude, "MoMo");

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
            Entities.Models.User user = await _accountService.GetUserAsync(User);

            Entities.Models.User userInclude = await _accountService.GetUserByUserIdAsync(user.Id);

            IList<Entities.Models.CartItem> carts = await _cartService.GetCartsByUserIdAsync(user.Id);

            using TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                PayPalHttp.HttpResponse captureOrderResponse = await _payPalService.PayPalCaptureOrder(token, true);

                if (!(captureOrderResponse is null))
                {

                    PayPalCheckoutSdk.Orders.Order captureOrderResult = captureOrderResponse.Result<PayPalCheckoutSdk.Orders.Order>();

                    Entities.Models.Order order = await _orderService.AddOrderFromCartsAsync(carts, userInclude, "PayPal");

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
