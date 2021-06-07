using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IReceiptService
    {
        Task<bool> AddReceiptAsync(ImportWarehouse importWarehouse);
        Task<int> AddReceiptAsync(int id);
        Task<bool> AddReceiptDetailAsync(ImportWarehouseDetail importWarehouseDetail);

        Task<bool> AddReceiptRequestAsync(ReceiptRequest receiptRequest);
        Task<bool> AddReceiptDetailRequestsAsync(IList<ReceiptRequestDetail> receiptRequestDetails);
    }
}
