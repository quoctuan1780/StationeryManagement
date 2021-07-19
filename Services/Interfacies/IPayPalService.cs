using Entities.Models;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IPayPalService
    {
        PayPalEnvironment PayPalEnvironment();

        HttpClient Client();

        HttpClient Client(string refreshToken);

        OrderRequest BuildRequestBody(IList<CartItem> carts, User user, string host, string deliveryAddress);

        Task<HttpResponse> PayPalCreateOrder(string deliveryAddress, IList<CartItem> carts, User user, string host, bool debug = false);

        Task<HttpResponse> PayPalCreateOrder(string deliveryAddress, CartItem cart, User user, string host, bool debug = false);

        Task<HttpResponse> PayPalCaptureOrder(string OrderId, bool debug = false);

        Task<int> AddPayPalPaymentAsync(int orderId, string token, string payerID, string link, string captureId);
        Task<string> CapturesOrder(string orderId);
        Task<HttpResponse> CapturesRefund(string CaptureId, string Amount, bool debug = false);
    }   
}
