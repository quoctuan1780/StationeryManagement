using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IBannerService
    {
        Task<int> AddBannerAsync(Banner banner);
        Task<IList<Banner>> GetBannerAsync();
        Task<IList<Banner>> GetAllBannerAsync();
        Task<Banner> GetBannerByIdAsync(int bannerId);
        Task<int> UpdateBannerAsync(Banner banner);
        Task<int> DeleteBannerAsync(int bannerId);
    }
}
