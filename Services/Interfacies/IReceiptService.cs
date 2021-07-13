using Entities.Models;
using Services.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IReceiptService
    {
        Task<bool> AddReceiptAsync(ImportWarehouse importWarehouse);
        Task<int> AddReceiptAsync(int id);
        Task<ImportWarehouse> GetReceiptAfterUpdate(int id, List<int> AddQuantity, List<int> productDetailIds);
        Task<bool> AddReceiptDetailAsync(ImportWarehouseDetail importWarehouseDetail);
        Task<ImportWarehouse> GetReceiptAsync(int id);
        Task<List<ImportWarehouse>> GetReceiptsAsync();
        Task<bool> AddReceiptRequestAsync(ReceiptRequest receiptRequest);
        Task<int> AddReceiptRequestDetailAsync(List<ReceiptRequestDetail> receiptRequest);
        Task<IList<ReceiptRequest>> GetReceiptRequestsAsync();
        Task<int> DeleteReceiptRequestAsync(int requesrID);
        Task<int> CountAcceptedRequestReceiptAsync();
        Task<IList<PercentProcess>> GetListProcessReceiptAsync();
        Task<int> GetNumberOfProcessingReceiptAsync();
        Task<bool> AddReceiptDetailRequestsAsync(IList<ReceiptRequestDetail> receiptRequestDetails);
        Task<ReceiptRequest> GetReceiptRequestAsync(int id);
        Task<int> ApproveReceiptRequestAsync(int id);
        Task<int> RejectReceiptRequestAsync(int id);
    }
}
