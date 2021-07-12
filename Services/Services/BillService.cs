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
    public class BillService : IBillService
    {
        private readonly ShopDbContext _context;

        public BillService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddBillWithOrderIdAsync(int orderId)
        {
            var order = await _context.Orders.Include(x => x.OrderDetails).Where(x => x.OrderId == orderId).FirstOrDefaultAsync();

            var bill = new Bill()
            {
                CreateDate = DateTime.Now,
                PaymentMethod = order.PaymentMethod,
                UserId = order.UserId,
                Total = order.Total
            };

            await _context.Bills.AddAsync(bill);

            await _context.SaveChangesAsync();
            
            var billDetails = new List<BillDetail>();

            foreach(var item in order.OrderDetails)
            {
                billDetails.Add(new BillDetail()
                {
                    BillId = bill.BillId,
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
