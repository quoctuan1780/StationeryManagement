using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IMoMoService
    {
        string SendPaymentRequest(string endPoint, string postJsonString);
        string SignSHA256(string message, string key);
        string MoMoCheckout(string total, string orderInfo, string email, string hostName);
        Task<int> AddMoMoPaymentAsync(int orderId, string moMoOrderId, string payType, string responseTime);
    }
}
