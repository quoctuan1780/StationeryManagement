using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class BillDetailService : IBillDetailService
    {
        private readonly ShopDbContext _context;

        public BillDetailService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddBillDetailsAsync(int billId, int orderId)
        {
            var order = await _context.Orders.Include(x => x.OrderDetails).Where(x => x.OrderId == orderId).FirstOrDefaultAsync();
            var billDetails = new List<BillDetail>();

            foreach (var item in order.OrderDetails)
            {
                billDetails.Add(new BillDetail()
                {
                    BillId = billId,
                    SalePrice = item.SalePrice,
                    Price = item.Price,
                    ProductDetailId = item.ProductDetailId,
                    Quantity = item.Quantity
                });
            }

            await _context.BillDetails.AddRangeAsync(billDetails);

            return await _context.SaveChangesAsync();
        }
    }
}
