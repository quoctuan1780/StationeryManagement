using Common;
using Entities.Data;
using Entities.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Services.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly IConfiguration _configuration;
        private readonly ShopDbContext _context;

        public PayPalService(IConfiguration configuration, ShopDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public HttpClient Client()
        {
            return new PayPalHttpClient(PayPalEnvironment());
        }

        public HttpClient Client(string refreshToken)
        {
            return new PayPalHttpClient(PayPalEnvironment(), refreshToken);
        }

        public PayPalEnvironment PayPalEnvironment()
        {
            return new SandboxEnvironment(
                System.Environment.GetEnvironmentVariable(Constant.KEY_PAYPAL_CLIENT_ID)
                            ?? _configuration[Constant.PAYPAL_CLIENT_ID],

                System.Environment.GetEnvironmentVariable(Constant.KEY_CONFIRM_EMAIL_SUCCESS)
                            ?? _configuration[Constant.PAYPAL_SECRET]);
        }

        public OrderRequest BuildRequestBody(IList<CartItem> carts, User user, string host, string deliveryAddress)
        {
            var items = new List<Item>();

            decimal total = 0;

            foreach (var item in carts)
            {
                total += Math.Round((item.Price / (decimal)Constant.EXCHANGE_RATE_USD), 2) * item.Quantity;

                items.Add(new Item()
                {
                    Name = item.ProductDetail.Product.ProductName,
                    Sku = Constant.PAYPAL_SKU,
                    UnitAmount = new Money
                    {
                        CurrencyCode = Constant.CURRENCY_USE,
                        Value = Math.Round((item.Price / (decimal)Constant.EXCHANGE_RATE_USD), 2)
                                .ToString().Replace(Constant.COMMA, Constant.DOT)
                    },
                    Tax = new Money
                    {
                        CurrencyCode = Constant.CURRENCY_USE,
                        Value = Constant.ZERO
                    },
                    Quantity = item.Quantity.ToString(),
                    Category = Constant.PAYPAL_PHYSICAL_GOODS
                });
            }

            var orderRequest = new OrderRequest()
            {
                CheckoutPaymentIntent = Constant.PAYPAL_CAPTURE,

                ApplicationContext = new ApplicationContext
                {
                    CancelUrl = $"{host}/Order/PayPalFail",
                    ReturnUrl = $"{host}/Order/PayPalSuccess?deliveryAddress={deliveryAddress}",
                },
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest{

                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = Constant.CURRENCY_USE,
                            Value = total.ToString().Replace(Constant.COMMA, Constant.DOT),
                            AmountBreakdown = new AmountBreakdown
                            {
                                ItemTotal = new Money
                                {
                                    CurrencyCode = Constant.CURRENCY_USE,
                                    Value = total.ToString()
                                },
                                Shipping = new Money
                                {
                                    CurrencyCode = Constant.CURRENCY_USE,
                                    Value = Constant.ZERO
                                },
                                Handling = new Money
                                {
                                    CurrencyCode = Constant.CURRENCY_USE,
                                    Value = Constant.ZERO
                                },
                                TaxTotal = new Money
                                {
                                    CurrencyCode = Constant.CURRENCY_USE,
                                    Value = Constant.ZERO
                                },
                                ShippingDiscount = new Money
                                {
                                    CurrencyCode = Constant.CURRENCY_USE,
                                    Value = Constant.ZERO
                                }
                            }
                        },
                        Items = items,
                        ShippingDetail = new ShippingDetail
                        {
                            Name = new Name
                            {
                                FullName = user.FullName
                            },
                            AddressPortable = new AddressPortable
                            {
                                AddressLine1 = HttpUtility.UrlDecode(deliveryAddress),
                                AdminArea2 = Constant.VN_NAME,
                                AdminArea1 = Constant.VN_CODE,
                                PostalCode = Constant.ZERO,
                                CountryCode = Constant.VN_CODE
                            }
                        }
                    }
                }
            };

            return orderRequest;
        }

        public async Task<HttpResponse> PayPalCreateOrder(string deliveryAddress, IList<CartItem> carts, User user, string host, bool debug = false)
        {
            try
            {
                var request = new OrdersCreateRequest();
                request.Headers.Add(Constant.PAYPAL_HEADER_PREFER, Constant.PAYPAL_HEADER_RETURN);
                request.RequestBody(BuildRequestBody(carts, user, host, deliveryAddress));
                HttpResponse response = await Client().Execute(request);

                return response;
            }
            catch
            {
                return null;
            }
        }

        public async Task<HttpResponse> PayPalCreateOrder(string deliveryAddress, CartItem cart, User user, string host, bool debug = false)
        {
            try
            {
                var request = new OrdersCreateRequest();
                request.Headers.Add(Constant.PAYPAL_HEADER_PREFER, Constant.PAYPAL_HEADER_RETURN);
                request.RequestBody(BuildRequestBody(cart, user, host, deliveryAddress));
                HttpResponse response = await Client().Execute(request);

                return response;
            }
            catch
            {
                return null;
            }
        }

        public async Task<HttpResponse> PayPalCaptureOrder(string OrderId, bool debug = false)
        {
            try
            {
                var request = new OrdersCaptureRequest(OrderId);
                request.Prefer(Constant.PAYPAL_HEADER_RETURN);
                request.RequestBody(new OrderActionRequest());
                HttpResponse response = await Client().Execute(request);
                return response;
            }
            catch
            {
                return null;
            }
        }



        public async Task<int> AddPayPalPaymentAsync(int orderId, string token, string payerID, string link, string captureId)
        {
            var payPlPayment = new PayPalPayment()
            {
                OrderId = orderId,
                LinkDetail = link,
                PayerId = payerID,
                Token = token,
                CaptureId = captureId
            };

            await _context.AddAsync(payPlPayment);

            return await _context.SaveChangesAsync();
        }

        public async Task<string> CapturesOrder(string orderId)
        {
            var request = new OrdersCaptureRequest(orderId);
            request.RequestBody(new OrderActionRequest());
            HttpResponse response = await Client().Execute(request);
            var result = response.Result<PayPalCheckoutSdk.Orders.Order>();

            var json = new JObject
            {
                { "CaptureId", result.Id},
                { "OrderId", orderId}
            };
            return json.ToString();
        }

        public async Task<HttpResponse> CapturesRefund(string captureId, string Amount, bool debug = false)
        {
            var request = new PayPalCheckoutSdk.Payments.CapturesRefundRequest(captureId);
            request.Prefer(Constant.PAYPAL_HEADER_RETURN);
            var refundRequest = new PayPalCheckoutSdk.Payments.RefundRequest()
            {
                Amount = new PayPalCheckoutSdk.Payments.Money
                {
                    Value = Amount,
                    CurrencyCode = Constant.CURRENCY_USE
                }
            };
            request.RequestBody(refundRequest);
            var response = await Client().Execute(request);

            return response;
        }

        private static OrderRequest BuildRequestBody(CartItem cart, User user, string host, string deliveryAddress)
        {
            var items = new List<Item>();

            decimal total = 0;

            
            total += Math.Round((cart.Price / (decimal)Constant.EXCHANGE_RATE_USD), 2) * cart.Quantity;

            items.Add(new Item()
            {
                Name = cart.ProductDetail.Product.ProductName,
                Sku = Constant.PAYPAL_SKU,
                UnitAmount = new Money
                {
                    CurrencyCode = Constant.CURRENCY_USE,
                    Value = Math.Round((cart.Price / (decimal)Constant.EXCHANGE_RATE_USD), 2)
                            .ToString().Replace(Constant.COMMA, Constant.DOT)
                },
                Tax = new Money
                {
                    CurrencyCode = Constant.CURRENCY_USE,
                    Value = Constant.ZERO
                },
                Quantity = cart.Quantity.ToString(),
                Category = Constant.PAYPAL_PHYSICAL_GOODS
            });
            

            var orderRequest = new OrderRequest()
            {
                CheckoutPaymentIntent = Constant.PAYPAL_CAPTURE,

                ApplicationContext = new ApplicationContext
                {
                    CancelUrl = $"{host}/Order/PayPalFail",
                    ReturnUrl = $"{host}/Order/PayPalSuccessBuyNow?deliveryAddress={deliveryAddress}",
                },
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest{

                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = Constant.CURRENCY_USE,
                            Value = total.ToString().Replace(Constant.COMMA, Constant.DOT),
                            AmountBreakdown = new AmountBreakdown
                            {
                                ItemTotal = new Money
                                {
                                    CurrencyCode = Constant.CURRENCY_USE,
                                    Value = total.ToString()
                                },
                                Shipping = new Money
                                {
                                    CurrencyCode = Constant.CURRENCY_USE,
                                    Value = Constant.ZERO
                                },
                                Handling = new Money
                                {
                                    CurrencyCode = Constant.CURRENCY_USE,
                                    Value = Constant.ZERO
                                },
                                TaxTotal = new Money
                                {
                                    CurrencyCode = Constant.CURRENCY_USE,
                                    Value = Constant.ZERO
                                },
                                ShippingDiscount = new Money
                                {
                                    CurrencyCode = Constant.CURRENCY_USE,
                                    Value = Constant.ZERO
                                }
                            }
                        },
                        Items = items,
                        ShippingDetail = new ShippingDetail
                        {
                            Name = new Name
                            {
                                FullName = user.FullName
                            },
                            AddressPortable = new AddressPortable
                            {
                                AddressLine1 = HttpUtility.UrlDecode(deliveryAddress),
                                AdminArea2 = Constant.VN_NAME,
                                AdminArea1 = Constant.VN_CODE,
                                PostalCode = Constant.ZERO,
                                CountryCode = Constant.VN_CODE
                            }
                        }
                    }
                }
            };

            return orderRequest;
        }
    }
}
