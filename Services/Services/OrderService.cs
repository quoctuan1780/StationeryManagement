using Common;
using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<Order> AddOrderFromCartsAsync(IList<CartItem> cartItems, User user, string paymentMethod)
        {
            string address = user.Ward.WardName + " - " + user.Ward.District.DistrictName + " - " + user.Ward.District.Province.ProvinceName;

            decimal total = 0;

            foreach(var item in cartItems)
            {
                total += item.Quantity * item.Price;
            }

            var order = new Order()
            {
                Address = address,
                UserId = user.Id,
                PaymentMethod = paymentMethod,
                Status = Constant.STATUS_WAITING_CONFIRM,
                OrderDate = DateTime.Now,
                Total = total
            };

            await _context.AddAsync(order);

            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.Where(x => x.OrderId == orderId)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.ProductDetail)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductImages)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
