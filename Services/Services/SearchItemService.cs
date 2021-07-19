using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Interfacies;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<string> SearchAjaxAsync(string text)
        {
            var resultJson = new List<JObject>();
            if (int.TryParse(text, out int price))
            {
                var result = await SearchByPriceAsync(price);

                if (result != null && result.Any())
                {
                    foreach (var item in result.Take(10))
                    {
                        resultJson.Add(new JObject
                        {
                            { "ProductId", item.ProductId },
                            { "ProductName", item.ProductName }
                        });
                    }
                }
            }
            else
            {
                var result = await SearchByTextAsync(text);
                foreach (var item in result.Take(10))
                {
                    resultJson.Add(new JObject
                        {
                            { "ProductId", item.ProductId },
                            { "ProductName", item.ProductName }
                        });
                }
            }

            return JsonConvert.SerializeObject(resultJson);
        }

        public async Task<IList<Product>> SearchByPriceAsync(int? price)
        {
            return await _context.Products.Include(x => x.RatingDetails).ThenInclude(x => x.Rating).Include(x => x.Category).Include(x => x.ProductImages).Where(x => x.Price <= price).OrderBy(x => x.Price).ToListAsync();
        }


        public async Task<IList<Product>> SearchByTextAsync(string text)
        {
            if (text != null)
            {
                var listByCategory = await _context.Products.Include(x => x.RatingDetails).ThenInclude(x => x.Rating).Include(x => x.Category).Include(x => x.ProductImages).
                    Where(x => (x.Category.CategoryName.Contains(text) || x.ProductName.Contains(text)))
                    .OrderBy(x => x.Price).ToListAsync();


                return listByCategory;

            }
            else
            {
                return await _context.Products.Include(x => x.RatingDetails).ThenInclude(x => x.Rating).Include(x => x.Category).Include(x => x.ProductImages).ToListAsync();
            }
        }
    }
}
