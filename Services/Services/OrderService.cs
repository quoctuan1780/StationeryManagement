using Entities.Data;
using Entities.Models;
using Microsoft.Data.SqlClient;
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

            var province = deliveryAddress.Substring(deliveryAddress.LastIndexOf("-") + 2, deliveryAddress.Length - (deliveryAddress.LastIndexOf("-") + 2));

            var order = new Order()
            {
                Address = deliveryAddress,
                UserId = user.Id,
                PaymentMethod = paymentMethod,
                Status = STATUS_WAITING_CONFIRM,
                OrderDate = DateTime.Now,
                Total = total,
                Province = province
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
                .ThenInclude(x => x.ProductImages)
                .Include(x => x.MoMoPayment)
                .Include(x => x.PayPalPayment)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<Order>> GetOrdersAsync()
        {
            return await _context.Orders.Include(x => x.User).OrderBy(x => x.OrderDate).ToListAsync();
        }

        public async Task<IList<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(x => x.MoMoPayment)
                .Include(x => x.PayPalPayment).Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<IList<Order>> GetOrdersWaitDeliveryAsync(string userId, string customer = EMPTY, string pickedOrderDate = EMPTY, string address = EMPTY)
        {
            var result = _context.Orders.Include(x => x.User).Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS) && x.ShipperId.Equals(userId));

            if(customer != "null" && customer != EMPTY)
            {
                result = result.Where(x => x.UserId.Equals(customer));
            }
            if(address != "null" && address != EMPTY)
            {
                result = result.Where(x => x.Address.Contains(address));
            }
            if (pickedOrderDate != "null" && pickedOrderDate != EMPTY)
            {
                bool resultParse = DateTime.TryParse(pickedOrderDate, out DateTime date);
                if (resultParse)
                {
                    result = result.Where(x => x.ShipperPickOrderDate.Date.Equals(date.Date));
                }
            }

            return await result.OrderBy(x => x.OrderDate).ToListAsync();
        }

        public async Task<IList<Order>> GetOrdersWaitExportWarehouseAsync(string customer = EMPTY, string orderDate = EMPTY)
        {
            var result = _context.Orders.Include(x => x.OrderDetails).Include(x => x.User).Where(x => x.Status.Equals(STATUS_PREPARING_GOODS));

            if(customer != "null" && customer != EMPTY)
            {
                result = result.Where(x => x.UserId == customer);
            }
            if (orderDate != "null" && orderDate != EMPTY)
            {
                bool resultParse = DateTime.TryParse(orderDate, out DateTime date);
                if (resultParse)
                {
                    result = result.Where(x => x.OrderDate.Date.Equals(date.Date));
                }
            }

            return await result.OrderBy(x => x.OrderDate).ToListAsync();
        }

        public async Task<int> WarehouseManagementConfirmOrderAsync(int orderId, string userId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order is null)
            {
                return 0;
            }

            order.WarehouseId = userId;
            order.ModifiedBy = userId;
            order.ModifiedDate = DateTime.Now;
            order.ExportWarehouseDate = DateTime.Now;
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
            order.ModifiedBy = order.ShipperId;

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

        public IList<OrderHelper.OrderJoinHelper> GetOrdersWaitToPick(string customer = EMPTY, string orderDate = EMPTY, string address = EMPTY)
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
            if (customer != "null" && customer != EMPTY)
            {
                result = result.Where(x => x.Order.UserId == customer);
            }
            if (address != "null" && address != EMPTY)
            {
                result = result.Where(x => x.Order.Address.Contains(address));
            }
            if (orderDate != "null" && orderDate != EMPTY)
            {
                bool resultParse = DateTime.TryParse(orderDate, out DateTime date);
                if (resultParse)
                {
                    result = result.Where(x => x.Order.OrderDate.Date.Equals(date.Date));
                }
            }
            foreach (var item in result)
            {
                var temp = new OrderHelper.OrderJoinHelper
                {
                    Order = item.Order,
                    WarehouseManagementName = item.Name
                };

                orders.Add(temp);
            }

            return orders;
        }

        public async Task<string> ShipperConfirmPickOrdersAsync(IList<int> ordersId, string userId, IList<byte[]> rowVersions)
        {
            var orders = await _context.Orders.Where(x => ordersId.Contains(x.OrderId)).ToListAsync();
            
            if(orders is null)
            {
                return EMPTY;
            }
            string success = EMPTY;
            string fail = EMPTY;
            int i = 0;
            while(i < orders.Count)
                if (orders[i].RowVersion.SequenceEqual(rowVersions[i]))
                {
                    orders[i].ShipperId = userId;
                    orders[i].ModifiedBy = userId;
                    orders[i].ModifiedDate = DateTime.Now;
                    orders[i].ShipperPickOrderDate = DateTime.Now;
                    success += orders[i].OrderId.ToString() + " ";
                    i++;
                }
                else
                {
                    fail += orders[i].OrderId.ToString() + " ";
                    orders.RemoveAt(i);
                    rowVersions.RemoveAt(i);
                }

            var jsonResult = new JObject 
            {
                { SUCCESS, success },
                { FAIL, fail }
            };

            if (orders != null && orders.Count > 0)
            {
                _context.Orders.UpdateRange(orders);

                await _context.SaveChangesAsync();
            }

            return JsonConvert.SerializeObject(jsonResult);
        }

        public IList<OrderHelper.OrderJoinHelper> GetOrdersWaitDelivery(string userId = EMPTY)
        {
            var result = from order in _context.Orders
                         join user in _context.Users
                            on order.ShipperId equals user.Id
                        join user1 in _context.Users
                            on order.WarehouseId equals user1.Id
                         where order.Status.Equals(STATUS_ON_DELIVERY_GOODS)
                         select new
                         {
                             Order = order,
                             ShipperName = user.FullName,
                             WareHouseManamenentName = user1.FullName
                         };
            if (!userId.Equals(EMPTY))
            {
                result = result.Where(x => x.Order.ShipperId.Equals(userId));
            }

            var orders = new List<OrderHelper.OrderJoinHelper>();

            foreach(var item in result)
            {
                var temp = new OrderHelper.OrderJoinHelper
                {
                    Order = item.Order,
                    ShipperName = item.ShipperName,
                    WarehouseManagementName = item.WareHouseManamenentName
                };

                orders.Add(temp);
            }

            return orders;
        }

        public async Task<IList<Order>> GetOrdersWaitToConfirmAsync(string orderDate, string paymentMethod, string customer)
        {
            var result = _context.Orders.Include(x => x.User).Include(x => x.OrderDetails).Where(x => x.Status.Equals(STATUS_WAITING_CONFIRM));

            if (customer != "null" && customer != EMPTY)
            {
                result = result.Where(x => x.UserId == customer);
            }

            if (paymentMethod != "null" && paymentMethod != EMPTY)
            {
                result = result.Where(x => x.PaymentMethod == paymentMethod);
            }

            if (orderDate != "null" && orderDate != EMPTY)
            {
                bool resultParse = DateTime.TryParse(orderDate, out DateTime date);
                if (resultParse)
                {
                    result = result.Where(x => x.OrderDate.Date.Equals(date.Date));
                }
            }

            return await result.OrderBy(x => x.OrderDate).ToListAsync();
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
                         where o.CreateDate.Year == DateTime.Now.Year
                         group o by o.CreateDate.Month
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

        public async Task<IList<Order>> GetOrdersDeliveredAsync(string userId)
        {
            return await _context.Orders.Include(x => x.User).Where(x => x.ShipperId == userId && x.Status.Equals(STATUS_RECEIVED_GOODS)).ToListAsync();
        }

        public async Task<IList<WorkflowHistory>> GetOrderHistoryAsync(int orderId)
        {
            return await _context.WorkflowHistories.Where(x => x.RecordId == orderId.ToString()).OrderBy(x => x.CreatedDate).ToListAsync();
        }

        public IList<OrderHelper.OrderJoinHelper> GetOrderDelivered(string userId = EMPTY)
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
                if (!userId.Equals(EMPTY))
                {
                    result = result.Where(x => x.Order.ShipperId.Equals(userId));
                }

                foreach (var item in result)
                {
                    var temp = new OrderHelper.OrderJoinHelper
                    {
                        Order = item.Order,
                        ShipperName = item.ShipperName
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

        public IList<OrderHelper.OrderJoinHelper> GetDateTimeDeliveredByFilter(string customerId, string shipperName, string receivedDate, string userId = EMPTY)
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
            if (!userId.Equals(EMPTY))
            {
                result = result.Where(x => x.Order.ShipperId.Equals(userId));
            }

            if(customerId != "null" && customerId != EMPTY)
            {
                result = result.Where(x => x.Order.UserId == customerId);
            }

            if (shipperName != "null" && shipperName != EMPTY)
            {
                result = result.Where(x => x.ShipperName == shipperName);
            }

            if (receivedDate != "null" && receivedDate != EMPTY)
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
                        Order = item.Order,
                        ShipperName = item.ShipperName
                    };

                    orders.Add(temp);
                }
            }

            return orders;
        }

        public IList<OrderHelper.OrderJoinHelper> FilterOrder(string exportWarehouseDate, string receivedDeliveryDate, string warehouse, string shipper, string userId = EMPTY, string customer = EMPTY)
        {
            var result = from order in _context.Orders.Include(x => x.User)
                         join user in _context.Users
                            on order.ShipperId equals user.Id
                        join user1 in _context.Users
                            on order.WarehouseId equals user1.Id
                         where order.Status.Equals(STATUS_ON_DELIVERY_GOODS)
                         orderby order.ReceivedDate descending
                         select new
                         {
                             Order = order,
                             ShipperName = user.FullName,
                             WarehouseManagementName = user1.FullName
                         };
            if (!userId.Equals(EMPTY))
            {
                result = result.Where(x => x.Order.ShipperId.Equals(userId));
            }

            if (warehouse != "null" && warehouse != EMPTY)
            {
                result = result.Where(x => x.WarehouseManagementName.Equals(warehouse));
            }

            if (shipper != "null" && shipper != EMPTY)
            {
                result = result.Where(x => x.ShipperName.Equals(shipper));
            }

            if (customer != "null" && customer != EMPTY)
            {
                result = result.Where(x => x.Order.UserId.Equals(customer));
            }

            if (exportWarehouseDate != null)
            {
                bool resultParse = DateTime.TryParse(exportWarehouseDate, out DateTime date);
                if (resultParse)
                {
                    result = result.Where(x => x.Order.ExportWarehouseDate.Date.Equals(date.Date));
                }
            }

            if (receivedDeliveryDate != null)
            {
                bool resultParse = DateTime.TryParse(receivedDeliveryDate, out DateTime date);
                if (resultParse)
                {
                    result = result.Where(x => x.Order.ShipperPickOrderDate.Date.Equals(date.Date));
                }
            }

            var orders = new List<OrderHelper.OrderJoinHelper>();
            if (result.Any())
            {
                foreach (var item in result)
                {
                    var temp = new OrderHelper.OrderJoinHelper
                    {
                        Order = item.Order,
                        ShipperName = item.ShipperName,
                        WarehouseManagementName = item.WarehouseManagementName
                    };

                    orders.Add(temp);
                }
            }

            return orders;
        }

        public IList<OrderHelper.OrderJoinHelper> FilterOrder(string exportWarehouseDate, string warehouse, string customer)
        {
            var result = from order in _context.Orders.Include(x => x.User)
                         join user in _context.Users
                            on order.WarehouseId equals user.Id
                         where order.Status.Equals(STATUS_WAITING_PICK_GOODS)
                         orderby order.ReceivedDate descending
                         select new
                         {
                             Order = order,
                             WarehouseManagementName = user.FullName,
                         };
            if (warehouse != "null" && warehouse != EMPTY)
            {
                result = result.Where(x => x.WarehouseManagementName.Equals(warehouse));
            }

            if (customer != "null" && customer != EMPTY)
            {
                result = result.Where(x => x.Order.User.Id.Equals(customer));
            }

            if (exportWarehouseDate != null)
            {
                bool resultParse = DateTime.TryParse(exportWarehouseDate, out DateTime date);
                if (resultParse)
                {
                    result = result.Where(x => x.Order.ExportWarehouseDate.Date.Equals(date.Date));
                }
            }


            var orders = new List<OrderHelper.OrderJoinHelper>();
            if (result.Any())
            {
                foreach (var item in result)
                {
                    var temp = new OrderHelper.OrderJoinHelper
                    {
                        Order = item.Order,
                        WarehouseManagementName = item.WarehouseManagementName
                    };

                    orders.Add(temp);
                }
            }

            return orders;
        }

        public async Task<IList<string>> GetAddressinOrdersAsync()
        {
            var result = from order in _context.Orders
                         group order by order.Address into g
                         select g.Key;

            return await result.ToListAsync();
        }

        public async Task<int> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);

            return await _context.SaveChangesAsync();
        }

        public IEnumerable<Order> FilterOrder(string customer = EMPTY, string paymentMethod = EMPTY, string status = STATUS_PENDING_ADMIN_CANCED_ORDER, bool debug = false)
        {
            var result = _context.Orders.Include(x => x.User).Where(x => x.Status.Equals(status));

            if (customer != "null" && customer != EMPTY)
            {
                result = result.Where(x => x.User.Id.Equals(customer));
            }

            if (paymentMethod != "null" && paymentMethod != EMPTY)
            {
                result = result.Where(x => x.PaymentMethod.Equals(paymentMethod));
            }

            return result;
        }

        public async Task<string> GetRevenueByYearAsync(int startDate, int endDate)
        {
            var orders = from o in _context.Orders
                         where o.OrderDate.Year >= startDate && o.OrderDate.Year <= endDate
                         group o by o.UserId into g
                         select new
                         {
                             UserId = g.Key,
                             QuantityOfOrder = g.Count(),
                             TotalOfOrder = g.Sum(x => x.Total),
                             QuantityOfOrderPaid = g.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS)).Count(),
                             TotalOfOrderPaid = g.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS)).Sum(x => x.Total),
                             QuantityOfOrderRejected = g.Where(x => x.Status.Equals(STATUS_CANCELED) || x.Status.Equals(STATUS_PENDING_ADMIN_CANCED_ORDER)).Count(),
                             TotalOfOrderRejected = g.Where(x => x.Status.Equals(STATUS_CANCELED) || x.Status.Equals(STATUS_PENDING_ADMIN_CANCED_ORDER)).Sum(x => x.Total)

                         };

            var result = new List<JObject>();

            if(orders != null)
            {
                var users = await _context.Users.Where(x => orders.Select(x => x.UserId).Contains(x.Id)).ToListAsync();
                foreach(var item in orders)
                {
                    var fullName = users.Where(x => x.Id == item.UserId).FirstOrDefault().FullName;
                    result.Add(new JObject
                    {
                        { "FullName", fullName },
                        { "QuantityOfOrder", item.QuantityOfOrder },
                        { "TotalOfOrder", item.TotalOfOrder },
                        { "QuantityOfOrderPaid", item.QuantityOfOrderPaid },
                        { "TotalOfOrderPaid", item.TotalOfOrderPaid },
                        { "QuantityOfOrderRejected", item.QuantityOfOrderRejected },
                        { "TotalOfOrderRejected", item.TotalOfOrderRejected },
                        { "Revenue", item.TotalOfOrderPaid }
                    });
                }
            }

            return JsonConvert.SerializeObject(result);
        }

        public async Task<IList<ExcelExportHelper>> GetRevenueExportExcelByYearAsync(int startDate, int endDate)
        {
            var orders = from o in _context.Orders
                         where o.OrderDate.Year >= startDate && o.OrderDate.Year <= endDate
                         group o by o.UserId into g
                         select new
                         {
                             UserId = g.Key,
                             QuantityOfOrder = g.Count(),
                             TotalOfOrder = g.Sum(x => x.Total),
                             QuantityOfOrderPaid = g.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS)).Count(),
                             TotalOfOrderPaid = g.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS)).Sum(x => x.Total),
                             QuantityOfOrderRejected = g.Where(x => x.Status.Equals(STATUS_CANCELED) || x.Status.Equals(STATUS_PENDING_ADMIN_CANCED_ORDER)).Count(),
                             TotalOfOrderRejected = g.Where(x => x.Status.Equals(STATUS_CANCELED) || x.Status.Equals(STATUS_PENDING_ADMIN_CANCED_ORDER)).Sum(x => x.Total)

                         };

            var result = new List<ExcelExportHelper>();

            if (orders != null)
            {
                var users = await _context.Users.Where(x => orders.Select(x => x.UserId).Contains(x.Id)).ToListAsync();
                foreach (var item in orders)
                {
                    var fullName = users.Where(x => x.Id == item.UserId).FirstOrDefault().FullName;
                    result.Add(new ExcelExportHelper()
                    {
                        FullName =  fullName,
                        QuantityOfOrder = item.QuantityOfOrder,
                        TotalOfOrder = item.TotalOfOrder,
                        QuantityOfOrderPaid = item.QuantityOfOrderPaid,
                        TotalOfOrderPaid = item.TotalOfOrderPaid ,
                        QuantityOfOrderRejected = item.QuantityOfOrderRejected,
                        TotalOfOrderRejected = item.TotalOfOrderRejected,
                        Revenue = item.TotalOfOrderPaid
                    });
                }
            }

            return result;
        }

        public async Task<string> GetRevenueByMonthAsync(DateTime startDate, DateTime endDate)
        {
            var orders = from o in _context.Orders
                         where o.OrderDate.Date >= startDate.Date && o.OrderDate.Date <= endDate.Date
                         group o by o.UserId into g
                         select new
                         {
                             UserId = g.Key,
                             QuantityOfOrder = g.Count(),
                             TotalOfOrder = g.Sum(x => x.Total),
                             QuantityOfOrderPaid = g.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS)).Count(),
                             TotalOfOrderPaid = g.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS)).Sum(x => x.Total),
                             QuantityOfOrderRejected = g.Where(x => x.Status.Equals(STATUS_CANCELED) || x.Status.Equals(STATUS_PENDING_ADMIN_CANCED_ORDER)).Count(),
                             TotalOfOrderRejected = g.Where(x => x.Status.Equals(STATUS_CANCELED) || x.Status.Equals(STATUS_PENDING_ADMIN_CANCED_ORDER)).Sum(x => x.Total)

                         };

            var result = new List<JObject>();

            if (orders != null)
            {
                var users = await _context.Users.Where(x => orders.Select(x => x.UserId).Contains(x.Id)).ToListAsync();
                foreach (var item in orders)
                {
                    var fullName = users.Where(x => x.Id == item.UserId).FirstOrDefault().FullName;
                    result.Add(new JObject
                    {
                        { "FullName", fullName },
                        { "QuantityOfOrder", item.QuantityOfOrder },
                        { "TotalOfOrder", item.TotalOfOrder },
                        { "QuantityOfOrderPaid", item.QuantityOfOrderPaid },
                        { "TotalOfOrderPaid", item.TotalOfOrderPaid },
                        { "QuantityOfOrderRejected", item.QuantityOfOrderRejected },
                        { "TotalOfOrderRejected", item.TotalOfOrderRejected },
                        { "Revenue", item.TotalOfOrderPaid }
                    });
                }
            }

            return JsonConvert.SerializeObject(result);
        }

        public async Task<IList<ExcelExportHelper>> GetRevenueExportExcelByMonthAsync(DateTime startDate, DateTime endDate)
        {
            var orders = from o in _context.Orders
                         where o.OrderDate.Date >= startDate.Date && o.OrderDate.Date <= endDate.Date
                         group o by o.UserId into g
                         select new
                         {
                             UserId = g.Key,
                             QuantityOfOrder = g.Count(),
                             TotalOfOrder = g.Sum(x => x.Total),
                             QuantityOfOrderPaid = g.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS)).Count(),
                             TotalOfOrderPaid = g.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS)).Sum(x => x.Total),
                             QuantityOfOrderRejected = g.Where(x => x.Status.Equals(STATUS_CANCELED) || x.Status.Equals(STATUS_PENDING_ADMIN_CANCED_ORDER)).Count(),
                             TotalOfOrderRejected = g.Where(x => x.Status.Equals(STATUS_CANCELED) || x.Status.Equals(STATUS_PENDING_ADMIN_CANCED_ORDER)).Sum(x => x.Total)

                         };

            var result = new List<ExcelExportHelper>();

            if (orders != null)
            {
                var users = await _context.Users.Where(x => orders.Select(x => x.UserId).Contains(x.Id)).ToListAsync();
                foreach (var item in orders)
                {
                    var fullName = users.Where(x => x.Id == item.UserId).FirstOrDefault().FullName;
                    result.Add(new ExcelExportHelper()
                    {
                        FullName = fullName,
                        QuantityOfOrder = item.QuantityOfOrder,
                        TotalOfOrder = item.TotalOfOrder,
                        QuantityOfOrderPaid = item.QuantityOfOrderPaid,
                        TotalOfOrderPaid = item.TotalOfOrderPaid,
                        QuantityOfOrderRejected = item.QuantityOfOrderRejected,
                        TotalOfOrderRejected = item.TotalOfOrderRejected,
                        Revenue = item.TotalOfOrderPaid
                    });
                }
            }

            return result;
        }

        public async Task<string> GetRevenueByCustomerAsync(DateTime startDate, DateTime endDate, string customer)
        {
            var orders = _context.Orders.Where(x => x.OrderDate.Date >= startDate.Date && x.OrderDate.Date <= endDate.Date);

            if(customer != "null")
            {
                orders = orders.Where(x => x.UserId.Equals(customer));
            }

            var orderGroup = from o in orders
                                 group o by o.UserId into g
                                 select new
                                 {
                                     UserId = g.Key,
                                     QuantityOfOrder = g.Count(),
                                     TotalOfOrder = g.Sum(x => x.Total),
                                     QuantityOfOrderPaid = g.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS)).Count(),
                                     TotalOfOrderPaid = g.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS)).Sum(x => x.Total),
                                     QuantityOfOrderRejected = g.Where(x => x.Status.Equals(STATUS_CANCELED) || x.Status.Equals(STATUS_PENDING_ADMIN_CANCED_ORDER)).Count(),
                                     TotalOfOrderRejected = g.Where(x => x.Status.Equals(STATUS_CANCELED) || x.Status.Equals(STATUS_PENDING_ADMIN_CANCED_ORDER)).Sum(x => x.Total)

                                 };

            var result = new List<JObject>();

            if (orderGroup != null)
            {
                var users = await _context.Users.Where(x => orders.Select(x => x.UserId).Contains(x.Id)).ToListAsync();
                foreach (var item in orderGroup)
                {
                    var fullName = users.Where(x => x.Id == item.UserId).FirstOrDefault().FullName;
                    result.Add(new JObject
                    {
                        { "FullName", fullName },
                        { "QuantityOfOrder", item.QuantityOfOrder },
                        { "TotalOfOrder", item.TotalOfOrder },
                        { "QuantityOfOrderPaid", item.QuantityOfOrderPaid },
                        { "TotalOfOrderPaid", item.TotalOfOrderPaid },
                        { "QuantityOfOrderRejected", item.QuantityOfOrderRejected },
                        { "TotalOfOrderRejected", item.TotalOfOrderRejected },
                        { "Revenue", item.TotalOfOrderPaid }
                    });
                }
            }

            return JsonConvert.SerializeObject(result);
        }

        public async Task<IList<ExcelExportHelper>> GetRevenueExportExcelByCustomerAsync(DateTime startDate, DateTime endDate, string customer)
        {
            var orders = _context.Orders.Where(x => x.OrderDate.Date >= startDate.Date && x.OrderDate.Date <= endDate.Date);

            if (customer != "null")
            {
                orders = orders.Where(x => x.UserId.Equals(customer));
            }

            var orderGroup = from o in orders
                         group o by o.UserId into g
                         select new
                         {
                             UserId = g.Key,
                             QuantityOfOrder = g.Count(),
                             TotalOfOrder = g.Sum(x => x.Total),
                             QuantityOfOrderPaid = g.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS)).Count(),
                             TotalOfOrderPaid = g.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS)).Sum(x => x.Total),
                             QuantityOfOrderRejected = g.Where(x => x.Status.Equals(STATUS_CANCELED) || x.Status.Equals(STATUS_PENDING_ADMIN_CANCED_ORDER)).Count(),
                             TotalOfOrderRejected = g.Where(x => x.Status.Equals(STATUS_CANCELED) || x.Status.Equals(STATUS_PENDING_ADMIN_CANCED_ORDER)).Sum(x => x.Total)

                         };

            var result = new List<ExcelExportHelper>();

            if (orderGroup != null)
            {
                var users = await _context.Users.Where(x => orders.Select(x => x.UserId).Contains(x.Id)).ToListAsync();
                foreach (var item in orderGroup)
                {
                    var fullName = users.Where(x => x.Id == item.UserId).FirstOrDefault().FullName;
                    result.Add(new ExcelExportHelper()
                    {
                        FullName = fullName,
                        QuantityOfOrder = item.QuantityOfOrder,
                        TotalOfOrder = item.TotalOfOrder,
                        QuantityOfOrderPaid = item.QuantityOfOrderPaid,
                        TotalOfOrderPaid = item.TotalOfOrderPaid,
                        QuantityOfOrderRejected = item.QuantityOfOrderRejected,
                        TotalOfOrderRejected = item.TotalOfOrderRejected,
                        Revenue = item.TotalOfOrderPaid
                    });
                }
            }

            return result;
        }

        public async Task<string> GetRevenueByProductAsync(string startDate, string endDate)
        {
            var conn = _context.Database.GetConnectionString();
            var result = new List<JObject>();

            using(var sqlConnection = new SqlConnection(conn))
            {
                var query = @"SELECT od.ProductDetailId, p.ProductName, pd.Color, pd.Quantity AS QuantityRemaining, SUM(od.Quantity) AS QuantitySold, SUM(od.Quantity * od.Price) AS TotalOfPrice
                        FROM [dbo].[OrderDetail] od JOIN [dbo].[Order] o ON od.OrderId = o.OrderId JOIN [dbo].[ProductDetails] pd ON od.ProductDetailId = pd.ProductDetailId JOIN [dbo].[Product] p
                        ON pd.ProductId = p.ProductId
                        WHERE o.OrderDate BETWEEN @startDate AND @endDate AND o.Status = N'Đã nhận hàng'
                        GROUP BY od.ProductDetailId, pd.Color, p.ProductName, pd.Quantity";


                await sqlConnection.OpenAsync();
                var cmd = new SqlCommand(query, sqlConnection);

                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);

                var resultQuery = await cmd.ExecuteReaderAsync();

                while(await resultQuery.ReadAsync())
                {
                    var obj = new JObject
                    {
                        { "ProductName", resultQuery["ProductName"].ToString() },
                        { "Color", resultQuery["Color"].ToString() },
                        { "QuantitySold", Convert.ToInt32(resultQuery["QuantitySold"]) },
                        { "QuantityRemaining", Convert.ToInt32(resultQuery["QuantityRemaining"]) },
                        { "TotalOfPrice", Convert.ToDecimal(resultQuery["TotalOfPrice"]) },
                    };

                    result.Add(obj);
                }
            }

            return JsonConvert.SerializeObject(result);
        }

        public async Task<IList<ExcelExportForProductHelper>> GetRevenueExportExcelByProductAsync(string startDate, string endDate)
        {
            var conn = _context.Database.GetConnectionString();
            var result = new List<ExcelExportForProductHelper>();

            using (var sqlConnection = new SqlConnection(conn))
            {
                var query = @"SELECT od.ProductDetailId, p.ProductName, pd.Color, pd.Quantity AS QuantityRemaining, SUM(od.Quantity) AS QuantitySold, SUM(od.Quantity * od.Price) AS TotalOfPrice
                        FROM [dbo].[OrderDetail] od JOIN [dbo].[Order] o ON od.OrderId = o.OrderId JOIN [dbo].[ProductDetails] pd ON od.ProductDetailId = pd.ProductDetailId JOIN [dbo].[Product] p
                        ON pd.ProductId = p.ProductId
                        WHERE o.OrderDate BETWEEN @startDate AND @endDate AND o.Status = N'Đã nhận hàng'
                        GROUP BY od.ProductDetailId, pd.Color, p.ProductName, pd.Quantity";


                await sqlConnection.OpenAsync();
                var cmd = new SqlCommand(query, sqlConnection);

                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);

                var resultQuery = await cmd.ExecuteReaderAsync();

                while (await resultQuery.ReadAsync())
                {
                    var obj = new ExcelExportForProductHelper()
                    {
                        ProductName = resultQuery["ProductName"].ToString(),
                        Color = resultQuery["Color"].ToString(),
                        QuantitySold = Convert.ToInt32(resultQuery["QuantitySold"]),
                        QuantityRemaining = Convert.ToInt32(resultQuery["QuantityRemaining"]),
                        Revenue = Convert.ToDecimal(resultQuery["TotalOfPrice"]),
                    };

                    result.Add(obj);
                }
            }

            return result;
        }

        public async Task<string> GetRevenueByProvinceAsync(DateTime startDate, DateTime endDate, string province)
        {
            var orders = _context.Orders.Where(x => x.OrderDate.Date >= startDate.Date && x.OrderDate.Date <= endDate.Date);

            if (province != "null")
            {
                orders = orders.Where(x => x.Province.Equals(province));
            }

            var orderGroup = from o in orders
                             group o by o.Province into g
                             select new
                             {
                                 Province = g.Key,
                                 QuantityOfOrder = g.Count(),
                                 TotalOfOrder = g.Sum(x => x.Total),
                                 QuantityOfOrderPaid = g.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS)).Count(),
                                 TotalOfOrderPaid = g.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS)).Sum(x => x.Total),
                                 QuantityOfOrderRejected = g.Where(x => x.Status.Equals(STATUS_CANCELED) || x.Status.Equals(STATUS_PENDING_ADMIN_CANCED_ORDER)).Count(),
                                 TotalOfOrderRejected = g.Where(x => x.Status.Equals(STATUS_CANCELED) || x.Status.Equals(STATUS_PENDING_ADMIN_CANCED_ORDER)).Sum(x => x.Total)

                             };

            var result = new List<JObject>();

            if (orderGroup != null && await orderGroup.AnyAsync())
            {
                foreach (var item in orderGroup)
                {
                    result.Add(new JObject
                    {
                        { "Province", item.Province },
                        { "QuantityOfOrder", item.QuantityOfOrder },
                        { "TotalOfOrder", item.TotalOfOrder },
                        { "QuantityOfOrderPaid", item.QuantityOfOrderPaid },
                        { "TotalOfOrderPaid", item.TotalOfOrderPaid },
                        { "QuantityOfOrderRejected", item.QuantityOfOrderRejected },
                        { "TotalOfOrderRejected", item.TotalOfOrderRejected },
                        { "Revenue", item.TotalOfOrderPaid }
                    });
                }
            }

            return JsonConvert.SerializeObject(result);
        }

        public async Task<IList<ExcelExportForProvinceHelper>> GetRevenueExportExcelByProvinceAsync(DateTime startDate, DateTime endDate, string province)
        {
            var orders = _context.Orders.Where(x => x.OrderDate.Date >= startDate.Date && x.OrderDate.Date <= endDate.Date);

            if (province != "null")
            {
                orders = orders.Where(x => x.Province.Equals(province));
            }

            var orderGroup = from o in orders
                             group o by o.Province into g
                             select new
                             {
                                 Province = g.Key,
                                 QuantityOfOrder = g.Count(),
                                 TotalOfOrder = g.Sum(x => x.Total),
                                 QuantityOfOrderPaid = g.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS)).Count(),
                                 TotalOfOrderPaid = g.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS)).Sum(x => x.Total),
                                 QuantityOfOrderRejected = g.Where(x => x.Status.Equals(STATUS_CANCELED) || x.Status.Equals(STATUS_PENDING_ADMIN_CANCED_ORDER)).Count(),
                                 TotalOfOrderRejected = g.Where(x => x.Status.Equals(STATUS_CANCELED) || x.Status.Equals(STATUS_PENDING_ADMIN_CANCED_ORDER)).Sum(x => x.Total)

                             };

            var result = new List<ExcelExportForProvinceHelper>();

            if (orderGroup != null && await orderGroup.AnyAsync())
            {
                foreach (var item in orderGroup)
                {
                    result.Add(new ExcelExportForProvinceHelper()
                    {
                        Province = item.Province,
                        QuantityOfOrder = item.QuantityOfOrder,
                        TotalOfOrder = item.TotalOfOrder,
                        QuantityOfOrderPaid = item.QuantityOfOrderPaid,
                        TotalOfOrderPaid = item.TotalOfOrderPaid,
                        QuantityOfOrderRejected = item.QuantityOfOrderRejected,
                        TotalOfOrderRejected = item.TotalOfOrderRejected,
                        Revenue = item.TotalOfOrderPaid
                    });
                }
            }

            return result;
        }

        public async Task<int> PrepareOrder(int orderId, int productDetailId)
        {
            var orderDetail = await _context.OrderDetails.Where(x => x.OrderId == orderId && x.ProductDetailId == productDetailId).FirstOrDefaultAsync();
            orderDetail.Status = STATUS_WAITING_PICK_GOODS;
            var listDetail = await _context.OrderDetails.Where(x => x.OrderId == orderId && x.Status == STATUS_WAITING_PICK_GOODS).ToListAsync();
            var order = await _context.Orders.Include(x => x.OrderDetails).Where(x => x.OrderId == orderId).FirstOrDefaultAsync();
            if (listDetail.Count == order.OrderDetails.Count)
            {
                order.Status = STATUS_WAITING_FOR_DELIVERING;

            }
            _context.OrderDetails.Update(orderDetail);
            _context.Orders.Update(order);
            return await _context.SaveChangesAsync();
        }

        #region Feature developer
        //public async Task<int> PrepareOrder(int OrderId, int ProductDetailId)
        //{
        //    using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //    {
        //        var orderDetail = await _context.OrderDetails.Where(x => x.ProductDetailId == ProductDetailId)
        //            .Where(x=> x.OrderId == OrderId)
        //            .FirstOrDefaultAsync();
        //        var productDetail = await _context.ProductDetails.Where(x => x.ProductDetailId == ProductDetailId).FirstOrDefaultAsync();                                         
        //        if(productDetail.RemainingQuantity < orderDetail.Quantity )
        //        {
        //            return 0;
        //        }
        //        else
        //        {
        //            productDetail.QuantityOrdered += orderDetail.Quantity;
        //            productDetail.RemainingQuantity -= orderDetail.Quantity;
        //            orderDetail.Status = STATUS_WAITING_PICK_GOODS;
        //            _context.Update(productDetail);

        //        }
        //        if(await _context.SaveChangesAsync() > 0)
        //        {
        //            transaction.Complete();
        //        }
        //    }
        //    return 1;
        //}

        //public async Task<int> RejectOrder(int id)
        //{
        //    var order = await _context.Orders.Include(x => x.OrderDetails).Where(x =>x.OrderId == id).FirstOrDefaultAsync();
        //    order.Status = STATUS_CANCELED;
        //    foreach(var item in order.OrderDetails)
        //    {
        //        item.Status = STATUS_CANCELED;
        //    }
        //    _context.Orders.Update(order);
        //    return await _context.SaveChangesAsync();
        //}

        //public async Task<int> ShipperConfirmPickOrdersAsync(IList<int> ordersId, string userId)
        //{
        //    var orders = await _context.Orders.Where(x => ordersId.Contains(x.OrderId)).ToListAsync();

        //    if (orders is null)
        //    {
        //        return 0;
        //    }

        //    foreach (var item in orders)
        //    {
        //        item.ShipperId = userId;
        //        item.ModifiedBy = userId;
        //        item.ModifiedDate = DateTime.Now;
        //        item.ShipperPickOrderDate = DateTime.Now;
        //    }

        //    _context.Orders.UpdateRange(orders);

        //    return await _context.SaveChangesAsync();
        //}
        #endregion
    }
}
