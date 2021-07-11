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
    public class SaleService : ISaleService
    {
        private readonly ShopDbContext _context;

        public SaleService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<Sale> AddSaleAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);

            await _context.SaveChangesAsync();

            return sale;
        }

        public async Task<int> DeleteSaleByIdAsync(int saleId)
        {
            var sale = await _context.Sales.Include(x => x.SaleDetails).Where(x => x.SaleId == saleId).FirstOrDefaultAsync();

            if(sale != null && sale.SaleDetails != null && sale.SaleDetails.Any())
            {
                if (sale.SaleEndDate.Date >= DateTime.Now.Date)
                {
                    var productIds = sale.SaleDetails.Select(x => x.ProductId);

                    var products = await _context.Products.Where(x => productIds.Contains(x.ProductId)).ToListAsync();

                    if (products != null && products.Any())
                    {
                        foreach (var item in products)
                        {
                            item.SalePrice = 0;
                        }

                        _context.Products.UpdateRange(products);
                    }

                    foreach(var item in sale.SaleDetails)
                    {
                        item.IsDeleted = true;
                    }

                    _context.UpdateRange(sale.SaleDetails);
                }
            }

            sale.IsDeleted = true;

            _context.Update(sale);

            return await _context.SaveChangesAsync();
        }

        public async Task<Sale> GetSaleByIdAsync(int saleId)
        {
            return await _context.Sales.Include(x => x.SaleDetails)
                .Where(x => x.SaleId == saleId)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<Sale>> GetSalesAsync()
        {
            return await _context.Sales.OrderByDescending(x => x.SaleStartDate.Date).ToListAsync();
        }

        public async Task<IList<string>> GetThreeSalesImageAsync()
        {
            return await _context.Sales.Where(x => x.SaleEndDate.Date >= DateTime.Now.Date && x.Image != null).Take(3).Select(x => x.Image).ToListAsync();
        }

        public async Task<Sale> UpdateSaleAsync(Sale sale)
        {
            var result = await _context.Sales.FindAsync(sale.SaleId);

            result.SaleName = sale.SaleName;
            result.SaleStartDate = sale.SaleStartDate;
            result.SaleEndDate = sale.SaleEndDate;
            result.Description = sale.Description;
            result.Discount = sale.Discount;
            result.Image = sale.Image;

            if (sale.SaleType.Equals(TYPE_SALE_FOR_ORDER))
            {
                result.SaleType = sale.SaleType;
                result.FromOrderPrice = sale.FromOrderPrice;
            }
            result.Image = sale.Image;

            _context.Sales.Update(result);
            await _context.SaveChangesAsync();

            return sale;
        }
    }
}
