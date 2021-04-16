using Common;
using Entities.Data;
using Entities.Models;
using Services.Interfacies;
using System;
using System.Collections.Generic;
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
    }
}
