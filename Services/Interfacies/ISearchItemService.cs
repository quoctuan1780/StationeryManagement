using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface ISearchItemService
    {
        Task<IList<Product>> SearchByPriceAsync(int? price);
        Task<IList<Product>> SearchByTextAsync(string text);
        Task<string> SearchAjaxAsync(string text);
    }
}
