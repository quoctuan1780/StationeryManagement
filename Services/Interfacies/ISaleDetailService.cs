using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface ISaleDetailService
    {
        Task<int> AddSaleDetailsAsync(IList<int> productIds, int saleId, DateTime startSaleDate, DateTime endSaleDate);
        Task<int> UpdateSaleDetailsAsync(IList<int> productIds, decimal discount, int saleId, DateTime startSaleDate, DateTime endSaleDate);
        Task<IList<int>> GetProductIdInSaleAsync(int saleId);
    }
}
