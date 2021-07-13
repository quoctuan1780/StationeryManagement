using Common;
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
            if (productDetail is null) return Constant.ERROR_CODE_NULL;

            await _context.AddRangeAsync(productDetail);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteProductsDetailAsync(IList<string> productsDetailId, int productId)
        {
            // check products detail id is null or not null
            if (productsDetailId.FirstOrDefault() is null) return 1;

            var productsDetailRemove = productsDetailId.FirstOrDefault().Split(Constant.COMMA);

            var productsDetail = await _context.ProductDetails.Where(x => x.ProductId == productId).ToListAsync();

            // check products detail is null or not null
            if (!(productsDetail.FirstOrDefault() is null))
            {
                var productRemove = new List<ProductDetail>();

                foreach (var item in productsDetailRemove)
                {
                    var checkParse = int.TryParse(item, out int result);

                    // check parse data to int type
                    if (!checkParse) return Constant.ERROR_CODE_CONVERT_TO_INT;
                    var productDetail = productsDetail.Where(x => x.ProductDetailId == result).FirstOrDefault();
                    productDetail.IsDeleted = true;
                    productRemove.Add(productDetail);
                }

                _context.ProductDetails.UpdateRange(productRemove);
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<ProductDetail> GetProductDetailByIdAsync(int productDetailId)
        {
            return await _context.ProductDetails.Where(x => x.ProductDetailId == productDetailId).FirstOrDefaultAsync();
        }

        public async Task<IList<ProductDetail>> GetProductDetailByIdAsync(IEnumerable<int> productDetailIds)
        {
            return await _context.ProductDetails.Where(x => productDetailIds.Contains(x.ProductDetailId)).ToListAsync();
        }

        public async Task<IList<ProductDetail>> GetProductsDetailByProductIdAsync(int productId)
        {
            return await _context.ProductDetails.Where(x => x.ProductId == productId).ToListAsync();
        }

        public async Task<int> UpdateProductsDetailAsync(IList<ProductDetail> productDetails)
        {
            if (productDetails is null) return Constant.ERROR_CODE_NULL;

            var productsDetailUpdate = new List<ProductDetail>();

            var productsDetail = _context.ProductDetails.Where(x => x.ProductId == productDetails.FirstOrDefault().ProductId);

            foreach (var item in productDetails)
            {
                var result = await productsDetail
                    .Where(x => x.ProductDetailId == item.ProductDetailId).FirstOrDefaultAsync();

                if (!(result is null))
                {
                    result.Color = item.Color;
                    result.Weight = item.Weight;
                    result.Origin = item.Origin;
                    result.Quantity = item.Quantity;
                    result.Price = item.Price;
                    productsDetailUpdate.Add(result);
                }
                else
                {
                    productsDetailUpdate.Add(new ProductDetail()
                    {
                        Color = item.Color,
                        Weight = item.Weight,
                        Origin = item.Origin,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        ProductId = item.ProductId
                    });
                }

            }

            _context.ProductDetails.UpdateRange(productsDetailUpdate);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateSalePriceProductDetailsAsync(IList<int> productIds, decimal discount)
        {
            if (productIds != null && productIds.Any())
            {
                var productDetails = _context.ProductDetails.Include(x => x.Product)
                                    .Where(x => productIds.Contains(x.Product.ProductId));

                foreach(var item in productDetails)
                {
                    item.SalePrice = item.Price - item.Price * discount / 100;
                }

                _context.ProductDetails.UpdateRange(productDetails);
            }

            return await _context.SaveChangesAsync();
        }
    }
}
