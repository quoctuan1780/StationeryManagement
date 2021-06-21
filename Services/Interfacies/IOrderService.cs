using Entities.Models;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IOrderService
    {
        Task<Order> AddOrderAsync(Order order);
        Task<string> ListPercentDeliveryAsync(); 
        Task<Order> AddOrderFromCartsAsync(IList<CartItem> cartItems, User user, string paymentMethod, string deliveryAddress);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IList<Order>> GetOrdersByUserIdAsync(string userId);
        Task<IList<Order>> GetOrdersAsync();
        Task<IList<Order>> GetOrdersWaitExportWarehouseAsync();
        IList<OrderHelper.OrderJoinHelper> GetOrdersWaitDelivery();
        Task<IList<Order>> GetOrdersWaitDeliveryAsync(string userId);
        IList<OrderHelper.OrderJoinHelper> GetOrdersWaitToPick();
        Task<IList<Order>> GetOrdersWaitToConfirmAsync();
        Task<IList<Order>> GetOrdersDeliveryAsync(string userId);
        Task<IList<Order>> GetOrdersDeliveredAsync(string userId);
        Task<int> AdminConfirmOrderAsync(int orderId);
        Task<int> WarehouseManagementConfirmOrderAsync(int orderId, string userId);
        Task<int> ShipperConfirmOrderAsync(int orderId);
        Task<int> ShipperConfirmDeliveryAsync(int orderId);
        Task<int> ShipperConfirmPickOrdersAsync(IList<int> ordersId, string userId);
        Task<int> CountNewAcceptedOrdersAsync();
        Task<int> CountOrdersWaitToPickAsync(); 
        Task<int> CountOrderWaitToDeliveryAsync();
        Task<int> CountOrdersDeliveringAsync();
        Task<int> CountOrdersDeliveredAsync();
        Task<string> GetTotalSalesPerMonthsAsync();
        Task<string> GetTotalPurchasePerMonthsAsync();
        Task<IList<WorkflowHistory>> GetOrderHistoryAsync(int orderId);
        IList<OrderHelper.OrderJoinHelper> GetOrderDelivered();
        IList<DateTime> GetDateTimeDelivered();
        IList<OrderHelper.OrderJoinHelper> GetDateTimeDeliveredByFilter(string customerId, string shipperName, string receivedDate);
    }
}
