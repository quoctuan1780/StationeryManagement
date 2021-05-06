using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IDeliveryAddressService
    {
        Task<int> AddDeliveryServiceAsync(string wardCode, string streetName, string userId);

        Task<IList<DeliveryAddress>> GetDeliveryAddressesAsync(string userId);
    }
}
