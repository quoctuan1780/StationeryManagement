using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class DeliveryAddressService : IDeliveryAddressService
    {
        private readonly ShopDbContext _context;

        public DeliveryAddressService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddDeliveryServiceAsync(string wardCode, string streetName, string userId)
        {
            var deliveryAddress = new DeliveryAddress()
            {
                StreetName = streetName,
                UserId = userId,
                WardCode = wardCode
            };

            await _context.DeliveryAddresses.AddAsync(deliveryAddress);

            return await _context.SaveChangesAsync();
        }

        public async Task<IList<DeliveryAddress>> GetDeliveryAddressesAsync(string userId)
        {
            return await _context.DeliveryAddresses
                        .Where(x => x.UserId == userId)
                        .Include(x => x.Ward)
                        .ThenInclude(x => x.District)
                        .ThenInclude(x => x.Province)
                        .ToListAsync(); 
        }
    }
}
