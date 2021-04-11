using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ProductService : IProductService
    {
        private readonly ShopDbContext _context;

        public ProductService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            await _context.AddAsync(product);

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<IList<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .ToListAsync();
        }

        public async Task<IList<string>> GetColorByIdAsync(int productId)
        {
            var listDetails = await _context.ProductDetails.Where(x => x.ProductId == productId).ToListAsync();
            var listColor = new List<string>();
            foreach(var item in listDetails)
            {
                listColor.Add(item.Color);
            }
            return listColor;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Where(x => x.ProductId == id)
                            .Include(x => x.Category)
                            .Include(x => x.ProductImages)
                            .Include(x => x.ProductDetails)
                            .FirstOrDefaultAsync();
        }

        public async Task<IList<ProductDetail>> GetProductWithDetailsAsync()
        {
            return await _context.ProductDetails.Include(x => x.Product).ToListAsync();
        }

        public async Task<bool> IsExistsProduct(Product product)
        {
            var result = await _context.Products.Where(x => x.ProductName == product.ProductName).FirstOrDefaultAsync();

            if(result is null)
            {
                return false;
            }

            return true;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            if (product is null) return null;

            _context.Products.Update(product);

            await _context.SaveChangesAsync();

            return product;
        }
    }
}
