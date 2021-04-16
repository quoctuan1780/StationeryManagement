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

        OrderRequest BuildRequestBody(IList<CartItem> carts, User user, string host);

        Task<HttpResponse> PayPalCreateOrder(IList<CartItem> carts, User user, string host, bool debug = false);

        Task<HttpResponse> PayPalCaptureOrder(string OrderId, bool debug = false);

        Task<int> AddPayPalPaymentAsync(int orderId, string token, string payerID, string link);
    }
}
