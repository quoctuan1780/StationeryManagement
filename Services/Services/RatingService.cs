using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Interfacies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class RatingService : IRatingService
    {
        private readonly ShopDbContext _context;

        public RatingService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddRatingAsync(RatingDetail ratingDetail)
        {
            await _context.RatingDetails.AddAsync(ratingDetail);

            return await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckExistsRatingAsync(int productId, string userId)
        {
            var result = await _context.RatingDetails.Where(x => x.ProductId == productId && x.UserId == userId).FirstOrDefaultAsync();

            if(result != null)
            {
                return true;
            }

            return false;
        }

        public async Task<IList<Rating>> GetRatingsAsync()
        {
            return await _context.Ratings.ToListAsync();
        }

        public async Task<IList<RatingDetail>> GetRatingsDetailAsync(int productId, int skip = 0)
        {
            return await _context.RatingDetails.Where(x => x.ProductId == productId)
                        .Include(x => x.User)
                        .Include(x => x.Rating).Skip(skip).Take(10).ToListAsync();
        }

        public async Task<string> GetRatingsDetailJsonAsync(int productId, int skip = 0)
        {
            var result =  await _context.RatingDetails.Where(x => x.ProductId == productId)
                        .Include(x => x.User)
                        .Include(x => x.Rating).Skip(skip).Take(10).ToListAsync();

            var json = new List<JObject>();

            if(result != null && result.Any())
            {
                foreach(var item in result)
                {
                    var obj = new JObject
                    {
                        { "UserImage", item.User.Image },
                        { "FullName", item.User.FullName },
                        { "Email", item.User.Email.Replace('+', 'a') },
                        { "RatingDate", item.RatingDate.Date.ToShortDateString() },
                        { "RatingId", item.RatingId },
                        { "Content", item.Content },
                        { "Rating", (double)item.Rating.RatingNumber / 5 * 100 }
                    };

                    json.Add(obj);
                }
            }

            return JsonConvert.SerializeObject(json);
        }
    }
}
