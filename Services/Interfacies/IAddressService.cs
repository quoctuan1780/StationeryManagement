using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IAddressService
    {
        Task<IList<Province>> GetProvincesAsync();

        Task<IList<District>> GetDistrictsByProvinceIdAsync(int provinceId);
    }
}
