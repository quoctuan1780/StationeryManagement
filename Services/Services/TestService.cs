using Entities.Data;
using Services.Interfacies;
using System.Threading.Tasks;

namespace Services.Services
{
    public class TestService : ITestService
    {
        private readonly ShopDbContext _context;

        public TestService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task UpdateProductTotalAsync()
        {
            var product = await _context.Products.FindAsync(3);
            product.Total++;
            _context.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
