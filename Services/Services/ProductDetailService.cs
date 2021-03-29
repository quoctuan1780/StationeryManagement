using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly ShopDbContext _context;

        public ProductDetailService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddProductsDetailAsync(IList<ProductDetail> productDetail)
        {
            await _context.AddRangeAsync(productDetail);

            return await _context.SaveChangesAsync();
        }

        public async Task<IList<ProductDetail>> GetProductsDetailByProductIdAsync(int productId)
        {
            return await _context.ProductDetails.Where(x => x.ProductId == productId).ToListAsync();
        }

        public async Task<int> UpdateProductsDetailAsync(IList<ProductDetail> productDetails)
        {
            _context.ProductDetails.UpdateRange(productDetails);

            return await _context.SaveChangesAsync();
        }
    }
}
