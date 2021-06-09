using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IHubService
    {
        Task<int> GetOrdersAsync();
        Task<int> GetWarehouseAsync();
        Task<string> GetAccountAsync();
        Task<string> GetRevenueAsync();
        Task<string> GetTopProductAsync();
        Task<string> GetTopCustomerAsync();
        Task<string> GetRevenueCurrentMonthAsync();
    }
}
