using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IFastDeliveryService
    {
        Task<string> CaculateFeeShipAsync(int fromDistrictId, int serviceId, int toDistrictId, int width, int height,
            int weight, int length, int fee);
    }
}
