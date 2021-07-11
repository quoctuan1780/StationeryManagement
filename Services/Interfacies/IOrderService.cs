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
        Task<int> UpdateOrderAsync(Order order);
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
        IEnumerable<Order> FilterOrder(string customer = EMPTY, string paymentMethod = EMPTY, string status = STATUS_PENDING_ADMIN_CANCED_ORDER, bool debug = false);
        Task<string> GetRevenueByYearAsync(int startDate, int endDate);
        Task<string> GetRevenueByMonthAsync(DateTime startDate, DateTime endDate);
        Task<string> GetRevenueByProductAsync(string startDate, string endDate);
        Task<string> GetRevenueByProvinceAsync(DateTime startDate, DateTime endDate, string province);
        Task<string> GetRevenueByCustomerAsync(DateTime startDate, DateTime endDate, string customer);
        Task<IList<ExcelExportHelper>> GetRevenueExportExcelByYearAsync(int startDate, int endDate);
        Task<IList<ExcelExportHelper>> GetRevenueExportExcelByMonthAsync(DateTime startDate, DateTime endDate);
        Task<IList<ExcelExportForProductHelper>> GetRevenueExportExcelByProductAsync(string startDate, string endDate);
        Task<IList<ExcelExportHelper>> GetRevenueExportExcelByCustomerAsync(DateTime startDate, DateTime endDate, string customer);
        Task<IList<ExcelExportForProvinceHelper>> GetRevenueExportExcelByProvinceAsync(DateTime startDate, DateTime endDate, string province);

        #region Feature Developer
        //Task<int> PrepareOrder(int orderId, int ProductDetailId);
        //Task<int> RejectOrder(int id);
        //Task<int> ShipperConfirmPickOrdersAsync(IList<int> ordersId, string userId);
        #endregion
    }
}
