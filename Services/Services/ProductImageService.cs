using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using static Common.Constant;

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

            var imagesRemove = images.FirstOrDefault().Split(COMMA);

            var productImages = await _context.ProductImages.Where(x => x.ProductId == id).ToListAsync();

            // can not find images of product by id product
            if (productImages is null) return ERROR_CODE_CANNOT_FIND_INFOR_BY_ID;

            var productRemove = new List<ProductImage>();

            foreach(var item in imagesRemove)
            {
                var image = productImages.Where(x => x.Image == item).FirstOrDefault();
                image.IsDeleted = true;
                productRemove.Add(image);
            }

            _context.ProductImages.UpdateRange(productRemove);

            return await _context.SaveChangesAsync();
        }

        public async Task<IList<ProductImage>> GetImagesByProductIdAsync(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId).ToListAsync();
        }
    }
}
