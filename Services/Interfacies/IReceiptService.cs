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
        Task<int> AddReceiptRequestDetailAsync(ReceiptRequestDetail receiptRequest);
        Task<IList<ReceiptRequest>> GetReceiptRequestsAsync();
        Task<int> DeleteReceiptRequest(int requesrID);
        Task<bool> AddReceiptDetailRequestsAsync(IList<ReceiptRequestDetail> receiptRequestDetails);
        Task<ReceiptRequest> GetReceiptRequestAsync(int id);
        Task<int> ApproveReceiptRequestAsync(int id);
        Task<int> RejectReceiptRequestAsync(int id);
    }
}
