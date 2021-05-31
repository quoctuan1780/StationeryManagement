using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IOrderService
    {
        Task<Order> AddOrderAsync(Order order);
        Task<Order> AddOrderFromCartsAsync(IList<CartItem> cartItems, User user, string paymentMethod, string deliveryAddress);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IList<Order>> GetOrdersByUserIdAsync(string userId);
        Task<IList<Order>> GetOrdersAsync();
        Task<IList<Order>> GetOrdersWaitExportWarehouseAsync();
        Task<IList<Order>> GetOrdersWaitDeliveryAsync();
        Task<IList<Order>> GetOrdersDeliveryAsync();
        Task<int> AdminConfirmOrderAsync(int orderId);
        Task<int> WarehouseManagementConfirmOrderAsync(int orderId);
        Task<int> ShipperConfirmOrderAsync(int orderId);
        Task<int> ShipperConfirmDeliveryAsync(int orderId);
    }
}
