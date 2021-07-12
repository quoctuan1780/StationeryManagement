using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IBillDetailService
    {
        Task<int> AddBillDetailsAsync(int billId, int orderId);
    }
}
