using Entities.Data;
using Entities.Models;
using Services.Interfacies;
using System.Collections.Generic;
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
            await _context.AddRangeAsync(productImages);

            return await _context.SaveChangesAsync();
        }
    }
}
