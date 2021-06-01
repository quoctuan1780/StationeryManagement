using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class SearchItemService : ISearchItemService
    {
        private readonly ShopDbContext _context;

        public SearchItemService(ShopDbContext context)
        {
            _context = context;
        }

        public Task<IList<Product>> SearchAjaxAsync(string text)
        {
            int price;
            if(int.TryParse(text,  out price))
            {
                return SearchByPriceAsync(price);
            }
            else
            {
                return SearchByTextAsync(text);
            }
        }

        public async Task<IList<Product>> SearchByPriceAsync(int? price)
        {
            return await _context.Products.Where(x => x.Price <= price).OrderBy(x => x.Price).ToListAsync();
        }


        public async Task<IList<Product>> SearchByTextAsync(string text)
        {
            if(text is not null)
            {
                var listByCategory = await _context.Products.Include(x => x.Category).Include(x => x.ProductImages).
                    Where(x => x.Category.CategoryName.Contains(text) ||  x.ProductName.Contains(text) )
                    .OrderBy(x => x.Price).ToListAsync();
                
                
                return listByCategory;

            }
            else
            {
                return await _context.Products.ToListAsync();
            }
            
        }
    }
}
