using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AddressService : IAddressService
    {
        private readonly ShopDbContext _context;

        public AddressService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<IList<District>> GetDistrictsByProvinceIdAsync(int provinceId)
        {
            return await _context.Districts.Where(x => x.ProvinceId == provinceId).ToListAsync();
        }

        public async Task<IList<Province>> GetProvincesAsync()
        {
            return await _context.Provinces.ToListAsync();
        }

        public async Task<IList<Ward>> GetWardsByDistrictIdAsync(int districtId)
        {
            return await _context.Wards.Where(x => x.DistrictId == districtId).ToListAsync();
        }
    }
}
