using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Helpers;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Common.Constant;

namespace Services.Services 
{
    public class OrderService : IOrderService
    {
        private readonly ShopDbContext _context;

        public OrderService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<Order> AddOrderAsync(Order order)
        {
            await _context.AddAsync(order);

            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order> AddOrderFromCartsAsync(IList<CartItem> cartItems, User user, string paymentMethod, string deliveryAddress)
        {
            var productDetailsId = cartItems.Select(x => x.ProductDetailId);
            var productDetails = await _context.ProductDetails.Where(x => productDetailsId.Contains(x.ProductDetailId)).ToListAsync();

            decimal total = 0;

            foreach(var item in cartItems)
            {
                var index = productDetails.FindIndex(x => x.ProductDetailId == item.ProductDetailId);

                productDetails[index].QuantityOrdered += item.Quantity;
                productDetails[index].RemainingQuantity -= item.Quantity;

                total += item.Quantity * item.Price;
            }

            var order = new Order()
            {
                Address = deliveryAddress,
                UserId = user.Id,
                PaymentMethod = paymentMethod,
                Status = STATUS_WAITING_CONFIRM,
                OrderDate = DateTime.Now,
                Total = total
            };

            await _context.Orders.AddAsync(order);

            _context.ProductDetails.UpdateRange(productDetails);

            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<int> AdminConfirmOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if(order is null)
            {
                return 0;
            }

            order.Status = STATUS_PREPARING_GOODS;
            order.ModifiedDate = DateTime.Now;

            _context.Orders.Update(order);

            return await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.Where(x => x.OrderId == orderId)
                .Include(x => x.User)
                .ThenInclude(x => x.Ward)
                .ThenInclude(x => x.District)
                .ThenInclude(x => x.Province)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.ProductDetail)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductImages.Where(x => x.IsDeleted == false))
                .FirstOrDefaultAsync();
        }

        public async Task<IList<Order>> GetOrdersAsync()
        {
            return await _context.Orders.Include(x => x.User).OrderBy(x => x.OrderDate).ToListAsync();
        }

        public async Task<IList<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<IList<Order>> GetOrdersWaitDeliveryAsync(string userId)
        {
            return await _context.Orders.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS) && x.ShipperId.Equals(userId)).Include(x => x.User).OrderBy(x => x.OrderDate).ToListAsync();
        }

        public async Task<IList<Order>> GetOrdersWaitExportWarehouseAsync()
        {
            return await _context.Orders.Where(x => x.Status.Equals(STATUS_PREPARING_GOODS)).Include(x => x.User).OrderBy(x => x.OrderDate).ToListAsync();
        }

        public async Task<int> WarehouseManagementConfirmOrderAsync(int orderId, string userId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order is null)
            {
                return 0;
            }

            order.WarehouseId = userId;
            order.Status = STATUS_WAITING_PICK_GOODS;

            _context.Orders.Update(order);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> ShipperConfirmOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order is null)
            {
                return 0;
            }

            order.Status = STATUS_ON_DELIVERY_GOODS;
            order.ModifiedDate = DateTime.Now;

            _context.Orders.Update(order);

            return await _context.SaveChangesAsync();
        }

        public async Task<IList<Order>> GetOrdersDeliveryAsync(string userId)
        {
            return await _context.Orders.Where(x => x.Status.Equals(STATUS_ON_DELIVERY_GOODS) && x.ShipperId.Equals(userId)).Include(x => x.User).OrderBy(x => x.OrderDate).ToListAsync();
        }

        public async Task<int> ShipperConfirmDeliveryAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order is null)
            {
                return 0;
            }

            order.Status = STATUS_RECEIVED_GOODS;
            order.ReceivedDate = DateTime.Now;
            order.ModifiedDate = DateTime.Now;

            _context.Orders.Update(order);

            return await _context.SaveChangesAsync();
        }

        public IList<OrderHelper.OrderJoinHelper> GetOrdersWaitToPick()
        {
            var result = from order in _context.Orders.Include(x => x.User)
                         join
                            user in _context.Users on order.WarehouseId equals user.Id
                         where order.Status.Equals(STATUS_WAITING_PICK_GOODS) && order.ShipperId == null
                         select new
                         {
                             Order = order,
                             Name = user.FullName
                         };

            var orders = new List<OrderHelper.OrderJoinHelper>();

            foreach (var item in result)
            {
                var temp = new OrderHelper.OrderJoinHelper
                {
                    order = item.Order,
                    Name = item.Name
                };

                orders.Add(temp);
            }

            return orders;
        }

        public async Task<int> ShipperConfirmPickOrdersAsync(IList<int> ordersId, string userId)
        {
            var orders = await _context.Orders.Where(x => ordersId.Contains(x.OrderId)).ToListAsync();
            
            if(orders is null)
            {
                return 0;
            }

            foreach(var item in orders)
            {
                item.ShipperId = userId;
                item.ModifiedBy = userId;
                item.ModifiedDate = DateTime.Now;
                item.ShipperPickOrderDate = DateTime.Now;
            }

            _context.Orders.UpdateRange(orders);

            return await _context.SaveChangesAsync();
        }

        public IList<OrderHelper.OrderJoinHelper> GetOrdersWaitDelivery()
        {
            var result = from order in _context.Orders
                         join user in _context.Users
                            on order.ShipperId equals user.Id
                         where order.Status.Equals(STATUS_ON_DELIVERY_GOODS)
                         select new
                         {
                             Order = order,
                             ShipperName = user.FullName
                         };
            var orders = new List<OrderHelper.OrderJoinHelper>();

            foreach(var item in result)
            {
                var temp = new OrderHelper.OrderJoinHelper
                {
                    order = item.Order,
                    Name = item.ShipperName
                };

                orders.Add(temp);
            }

            return orders;
        }

        public async Task<IList<Order>> GetOrdersWaitToConfirmAsync()
        {
            return await _context.Orders.Include(x => x.User).Where(x => x.Status.Equals(STATUS_WAITING_CONFIRM)).OrderBy(x => x.OrderDate).ToListAsync();
        }

        public async Task<IList<Order>> GetOrdersDeliveredAsync(string userId)
        {
            return await _context.Orders.Include(x => x.User).Where(x => x.ShipperId == userId && x.Status.Equals(STATUS_RECEIVED_GOODS)).ToListAsync();
        }

        public async Task<IList<WorkflowHistory>> GetOrderHistoryAsync(int orderId)
        {
            return await _context.WorkflowHistories.Where(x => x.RecordId == orderId.ToString()).OrderBy(x => x.CreatedDate).ToListAsync();
        }

        public IList<OrderHelper.OrderJoinHelper> GetOrderDelivered()
        {
            var result = from order in _context.Orders.Include(x => x.User)
                         join user in _context.Users
                            on order.ShipperId equals user.Id
                         where order.Status.Equals(STATUS_RECEIVED_GOODS) orderby order.ReceivedDate descending
                         select new
                         {
                             Order = order,
                             ShipperName = user.FullName
                         };
            var orders = new List<OrderHelper.OrderJoinHelper>();
            if (result.Any())
            {
                foreach (var item in result)
                {
                    var temp = new OrderHelper.OrderJoinHelper
                    {
                        order = item.Order,
                        Name = item.ShipperName
                    };

                    orders.Add(temp);
                }
            }

            return orders;
        }

        public IList<DateTime> GetDateTimeDelivered()
        {
            var result = from o in _context.Orders
                         group o by o.ReceivedDate.Date
                         into g
                         select new
                         {
                             ReceivedDate = g.Key
                         };
            var receivedDates = new List<DateTime>();
            foreach(var item in result)
            {
                receivedDates.Add(item.ReceivedDate);
            }
            return receivedDates;
        }

        public IList<OrderHelper.OrderJoinHelper> GetDateTimeDeliveredByFilter(string customerId, string shipperName, string receivedDate)
        {
            var result = from order in _context.Orders.Include(x => x.User)
                         join user in _context.Users
                            on order.ShipperId equals user.Id
                         where order.Status.Equals(STATUS_RECEIVED_GOODS)
                         orderby order.ReceivedDate descending
                         select new
                         {
                             Order = order,
                             ShipperName = user.FullName
                         };
            if(customerId != "null")
            {
                result = result.Where(x => x.Order.UserId == customerId);
            }

            if (shipperName != "null")
            {
                result = result.Where(x => x.ShipperName == shipperName);
            }

            if (receivedDate != "null")
            {
                bool resultParse = DateTime.TryParse(receivedDate, out DateTime receivedDateParsed);
                if (resultParse)
                {
                    result = result.Where(x => x.Order.ReceivedDate.Date.Equals(receivedDateParsed.Date));
                }
            }

            var orders = new List<OrderHelper.OrderJoinHelper>();
            if (result.Any())
            {
                foreach (var item in result)
                {
                    var temp = new OrderHelper.OrderJoinHelper
                    {
                        order = item.Order,
                        Name = item.ShipperName
                    };

                    orders.Add(temp);
                }
            }

            return orders;
        }
    }
}
