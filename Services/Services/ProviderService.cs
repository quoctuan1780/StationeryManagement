using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ProviderService : IProviderService
    {
        private readonly ShopDbContext _context;

        public ProviderService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<IList<Provider>> GetProviders()
        {
            return await _context.Providers.ToListAsync();
        }

        public async Task<IList<Provider>> GetProvidersByProduct(int productId)
        {
            IList<Provider> listProvider = await _context.Providers.Include(x => x.ImportWarehouseDetails.Where(i => i.ProductDetail.Product.ProductId == productId)).ToListAsync();
            if (listProvider.Count > 0)
                return listProvider;
            else 
                return await GetProviders();
            
        }
    }
}
