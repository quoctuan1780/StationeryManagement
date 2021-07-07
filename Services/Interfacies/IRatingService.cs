using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IRatingService
    {
        Task<int> AddRatingAsync(RatingDetail ratingDetail);
        Task<bool> CheckExistsRatingAsync(int productId, string userId);
        Task<IList<Rating>> GetRatingsAsync();
        Task<IList<RatingDetail>> GetRatingsDetailAsync(int productId, int skip = 0);
        Task<string> GetRatingsDetailJsonAsync(int productId, int skip = 0);
    }
}
