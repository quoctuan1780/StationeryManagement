using Entities.Models;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Common.Constant;

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
        Task<IList<string>> GetAddressinOrdersAsync();
        Task<IList<Order>> GetOrdersWaitExportWarehouseAsync(string customer = EMPTY, string orderDate = EMPTY);
        IList<OrderHelper.OrderJoinHelper> GetOrdersWaitDelivery(string userId = EMPTY);
        Task<IList<Order>> GetOrdersWaitDeliveryAsync(string userId, string customer = EMPTY, string pickedOrderDate = EMPTY, string address = EMPTY);
        IList<OrderHelper.OrderJoinHelper> GetOrdersWaitToPick(string customer = EMPTY, string orderDate = EMPTY, string address = EMPTY);
        Task<IList<Order>> GetOrdersWaitToConfirmAsync(string orderDate, string paymentMethod, string customer);
        Task<IList<Order>> GetOrdersDeliveryAsync(string userId);
        Task<IList<Order>> GetOrdersDeliveredAsync(string userId);
        Task<int> AdminConfirmOrderAsync(int orderId);
        Task<int> WarehouseManagementConfirmOrderAsync(int orderId, string userId);
        Task<int> ShipperConfirmOrderAsync(int orderId);
        Task<int> ShipperConfirmDeliveryAsync(int orderId);
        Task<string> ShipperConfirmPickOrdersAsync(IList<int> ordersId, string userId, IList<byte[]> rowVersions);
        
        Task<int> CountNewAcceptedOrdersAsync();
        Task<int> CountOrdersWaitToPickAsync(); 
        Task<int> CountOrderWaitToDeliveryAsync();
        Task<int> CountOrdersDeliveringAsync();
        Task<int> CountOrdersDeliveredAsync();
        Task<string> GetTotalSalesPerMonthsAsync();
        Task<string> GetTotalPurchasePerMonthsAsync();
        Task<IList<WorkflowHistory>> GetOrderHistoryAsync(int orderId);
        IList<OrderHelper.OrderJoinHelper> GetOrderDelivered(string userId = EMPTY);
        IList<DateTime> GetDateTimeDelivered();
        IList<OrderHelper.OrderJoinHelper> GetDateTimeDeliveredByFilter(string customerId, string shipperName, string receivedDate, string userId = EMPTY);
        IList<OrderHelper.OrderJoinHelper> FilterOrder(string exportWarehouseDate, string receivedDeliveryDate, string warehouse, string shipper, string userId = EMPTY, string customer = EMPTY);
        IList<OrderHelper.OrderJoinHelper> FilterOrder(string exportWarehouseDate, string warehouse, string customer);

        //Task<int> ShipperConfirmPickOrdersAsync(IList<int> ordersId, string userId);
    }
}
