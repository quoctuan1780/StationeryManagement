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
    public class SaleDetailService : ISaleDetailService
    {
        private readonly ShopDbContext _context;

        public SaleDetailService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddSaleDetailsAsync(IList<int> productIds, int saleId, DateTime startSaleDate, DateTime endSaleDate)
        {
            var saleDetails = new List<SaleDetail>();
            if (productIds != null && productIds.Any())
            {
                foreach (var item in productIds)
                {
                    saleDetails.Add(new SaleDetail()
                    {
                        SaleId = saleId,
                        ProductId = item,
                        SaleEndDate = endSaleDate,
                        SaleStartDate = startSaleDate
                    });
                }
            }

            await _context.SaleProducts.AddRangeAsync(saleDetails);

            return await _context.SaveChangesAsync();
        }

        public async Task<IList<int>> GetProductIdInSaleAsync(int saleId)
        {
            return await _context.SaleProducts.Where(x => x.IsDeleted == false && x.SaleId == saleId).Select(x => x.ProductId).ToListAsync();    
        }

        public async Task<int> UpdateSaleDetailsAsync(IList<int> productIds, decimal discount, int saleId, DateTime startSaleDate, DateTime endSaleDate)
        {
            var productsInSale = await _context.SaleProducts.Where(x => x.SaleId == saleId).ToListAsync();

            if (productsInSale != null && productsInSale.Any())
            {
                var productIdsOld = productsInSale.Select(x => x.ProductId);
                var products = await _context.Products.Where(x => x.IsDeleted == false && productIdsOld.Contains(x.ProductId)).ToListAsync();

                if (products != null && products.Any())
                {
                    foreach (var item in products)
                    {
                        item.SalePrice = 0;
                    }

                    _context.Products.UpdateRange(products);
                }
            }

            _context.SaleProducts.RemoveRange(productsInSale);

            var saleDetails = new List<SaleDetail>();

            if (productIds != null)
            {
                foreach (var item in productIds)
                {
                    saleDetails.Add(new SaleDetail()
                    {
                        SaleId = saleId,
                        ProductId = item,
                        SaleStartDate = startSaleDate,
                        SaleEndDate = endSaleDate
                    });
                }

                await _context.SaleProducts.AddRangeAsync(saleDetails);

                var productsUpdate = await _context.Products.Where(x => x.IsDeleted == false && productIds.Contains(x.ProductId)).ToListAsync();

                if (productsUpdate != null && productsUpdate.Any())
                {
                    foreach (var item in productsUpdate)
                    {
                        item.SalePrice = item.Price - item.Price * discount / 100;
                    }

                    _context.Products.UpdateRange(productsUpdate);
                }
            }

            return await _context.SaveChangesAsync();
        }
    }
}
