using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IHubShipperService
    {
        Task<int> GetOrderWaitToPickAsync();
        Task<int> GetOrderWaitToConfirmDeliveryAsync(string userId);
        Task<int> GetOrderDeliveringAsync(string userId);
        Task<int> GetOrderDeliveredAsync(string userId);
        Task<string> GetOrderDeliveringOrDeliveriedAsync(string userId);
    }
}
