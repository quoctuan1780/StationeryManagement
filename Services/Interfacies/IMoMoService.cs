using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IMoMoService
    {
        Task<string> SendPaymentRequestAsync(string endPoint, string postJsonString);
        string SignSHA256(string message, string key);
        Task<string> MoMoCheckoutAsync(string total, string orderInfo, string email, string hostName, string deliveryAddress);
        Task<string> MoMoCheckoutAsync(string total, string orderInfo, string email, string hostName);
        Task<int> AddMoMoPaymentAsync(int orderId, string moMoOrderId, string payType, string responseTime, string amount, string transId);
        Task<string> RefundMoneyAsync(string orderMomoId, string transId, string moneyRefund);
    }
}
