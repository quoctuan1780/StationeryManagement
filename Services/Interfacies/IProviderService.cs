using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IProviderService
    {
        Task<IList<Provider>> GetProvidersByProduct(int productId);
        Task<IList<Provider>> GetProviders();
    }
}
