using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IList<Order>> GetOrdersWaitDeliveryAsync()
        {
            return await _context.Orders.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS)).Include(x => x.User).OrderBy(x => x.OrderDate).ToListAsync();
        }

        public async Task<IList<Order>> GetOrdersWaitExportWarehouseAsync()
        {
            return await _context.Orders.Where(x => x.Status.Equals(STATUS_PREPARING_GOODS)).Include(x => x.User).OrderBy(x => x.OrderDate).ToListAsync();
        }

        public async Task<int> WarehouseManagementConfirmOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order is null)
            {
                return 0;
            }

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

        public async Task<IList<Order>> GetOrdersDeliveryAsync()
        {
            return await _context.Orders.Where(x => x.Status.Equals(STATUS_ON_DELIVERY_GOODS)).Include(x => x.User).OrderBy(x => x.OrderDate).ToListAsync();
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
    }
}
