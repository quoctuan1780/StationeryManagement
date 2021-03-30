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
    public class ProductImageService : IProductImageService
    {
        private readonly ShopDbContext _context;

        public ProductImageService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddListImagesAsync(IList<ProductImage> productImages)
        {
            if (productImages is null) return 1;

            await _context.AddRangeAsync(productImages);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteListImagesOfProductByNameAsync(IList<string> images, int id)
        {
            if (images.FirstOrDefault() is null) return 1;

            var imagesRemove = images.FirstOrDefault().Split(Constant.COMMA);

            var productImages = await _context.ProductImages.Where(x => x.ProductId == id).ToListAsync();

            // can not find images of product by id product
            if (productImages is null) return Constant.ERROR_CODE_CANNOT_FIND_INFOR_BY_ID;

            var productRemove = new List<ProductImage>();

            foreach(var item in imagesRemove)
            {
                productRemove.Add(
                    productImages.Where(x => x.Image == item).FirstOrDefault()
                );
            }

            _context.ProductImages.RemoveRange(productRemove);

            return await _context.SaveChangesAsync();
        }
    }
}
