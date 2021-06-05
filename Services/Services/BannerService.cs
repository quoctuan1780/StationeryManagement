using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class BannerService : IBannerService
    {
        private readonly ShopDbContext _context;

        public BannerService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddBannerAsync(Banner banner)
        {
            await _context.Banners.AddAsync(banner);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteBannerAsync(int bannerId)
        {
            var banner = await _context.Banners.FindAsync(bannerId);

            if(banner is null)
            {
                return 0;
            }

            banner.IsDeleted = true;

            _context.Banners.Update(banner);

            return await _context.SaveChangesAsync();
        }

        public async Task<IList<Banner>> GetBannerAsync()
        {
            return await _context.Banners.Where(x => x.IsDeleted == false && x.EndDate >= DateTime.Now).ToListAsync();
        }

        public async Task<Banner> GetBannerByIdAsync(int bannerId)
        {
            return await _context.Banners.FindAsync(bannerId);
        }

        public async Task<int> UpdateBannerAsync(Banner banner)
        {
            _context.Banners.Update(banner);

            return await _context.SaveChangesAsync();
        }
    }
}
