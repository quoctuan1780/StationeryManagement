using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

            if( await _context.SaveChangesAsync() > 0)
            {
                _context.SavedChanges += new EventHandler<SavedChangesEventArgs>(OrderedSignal);
            }

            return order;
        }

        public void OrderedSignal(object sender, EventArgs e)
        {
            
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
                productDetails[index].RemainingQuantity += productDetails[index].Quantity - item.Quantity;

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

        public async Task<int> CountNewAcceptedOrdersAsync()
        {
            var result = await _context.Orders.Where(x => x.Status.Equals(STATUS_PREPARING_GOODS)).ToListAsync();
            return result.Count;        
        }

        public async Task<int> CountOrdersWaitToPickAsync()
        {
            var result = await _context.Orders.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS)).ToListAsync();
            return result.Count;
        }

        public async Task<int> CountOrderWaitToDeliveryAsync()
        {
            var result = await _context.Orders.Where(x => x.Status.Equals(STATUS_WAITING_FOR_DELIVERING)).ToListAsync();
            return result.Count;
        }

        public async Task<int> CountOrdersDeliveringAsync()
        {
            var result = await _context.Orders.Where(x => x.Status.Equals(STATUS_ON_DELIVERY_GOODS)).ToListAsync();
            return result.Count;
        }

        public async Task<int> CountOrdersDeliveredAsync()
        {
            var result = await _context.Orders.Where(x => x.Status.Equals(STATUS_WAITING_EVALUATE)).ToListAsync();
            return result.Count;
        }

        public async Task<string> ListPercentDeliveryAsync()
        {
            var orders = from o in _context.Orders
                         group o by o.Status
                         into g
                         select new
                         {
                             Status = g.Key,
                             CountStatus = g.Select(x => x.OrderId).Count()
                         };
            var total = orders.Sum(x => x.CountStatus);
            var list = new List<DeliveryChart>();
            if (await orders.AnyAsync())
            {
                foreach (var order in orders)
                {
                    var obj = new DeliveryChart()
                    {
                        y = Math.Round((double)order.CountStatus / total * 100, 2),
                        name = order.Status
                    };
                    list.Add(obj);                }
            }
            return JsonConvert.SerializeObject(list);
        }

        public async Task<string> GetTotalSalesPerMonthsAsync()
        {
            var orders = from o in _context.Orders
                         where o.OrderDate.Year == DateTime.Now.Year
                         group o by o.OrderDate.Month
                         into g
                         select new
                         {
                             Month = g.Key,
                             Total = g.Sum(x => x.Total)
                         };
            
            var list = new List<SalesPurchases>();
            if (await orders.AnyAsync())
            {
                foreach (var order in orders)
                {
                    var obj = new SalesPurchases()
                    {
                        label = order.Month.ToString(),
                        y = order.Total
                    };
                    list.Add(obj);
                }
            }
            return JsonConvert.SerializeObject(list);
        }

        public async Task<string> GetTotalPurchasePerMonthsAsync()
        {
            var orders = from o in _context.ImportWarehouses
                         where o.ImportDate.Year == DateTime.Now.Year
                         group o by o.ImportDate.Month
                         into g
                         select new
                         {
                             Month = g.Key,
                             Total = g.Sum(x => x.Total)
                         };

            var list = new List<SalesPurchases>();
            if (await orders.AnyAsync())
            {
                foreach (var order in orders)
                {
                    var obj = new SalesPurchases()
                    {
                        label = order.Month.ToString(),
                        y = order.Total
                    };
                    list.Add(obj);
                }
            }
            return JsonConvert.SerializeObject(list);
        }
    }
}
