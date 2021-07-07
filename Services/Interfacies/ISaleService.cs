using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface ISaleService
    {
        Task<Sale> AddSaleAsync(Sale sale);
        Task<Sale> UpdateSaleAsync(Sale sale);
        Task<IList<string>> GetThreeSalesImageAsync();
        Task<IList<Sale>> GetSalesAsync();
        Task<int> DeleteSaleByIdAsync(int saleId);
        Task<Sale> GetSaleByIdAsync(int saleId);
    }
}
