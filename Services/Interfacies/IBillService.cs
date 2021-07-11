using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IBillService
    {
        Task<int> AddBillWithOrderIdAsync(int orderId);
    }
}
