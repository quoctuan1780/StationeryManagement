using Entities.Data;
using Entities.Models;
using Services.Interfacies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly ShopDbContext _context;

        public OrderDetailService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddOrderDetailAsync(Order order, IList<CartItem> cartItems)
        {
            var orderDetails = new List<OrderDetail>();

            foreach(var item in cartItems)
            {
                orderDetails.Add(new OrderDetail()
                {
                    OrderId = order.OrderId,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductDetailId = item.ProductDetailId,
                    SalePrice = 0
                });
            }

            await _context.AddRangeAsync(orderDetails);

            return await _context.SaveChangesAsync();
        }
    }
}
