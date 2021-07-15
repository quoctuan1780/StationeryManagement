using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Transactions;

namespace Services.Services
{
    public class UserConnectionService : IUserConnectionService
    {
        private readonly ShopDbContext _context;

        public UserConnectionService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task AddUserConnectionAsync(string userConnectionId, int productId)
        {
            var checkExistsConnectionId = await IsExistsConnectionIdAsync(userConnectionId);

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                if (!checkExistsConnectionId)
                {
                    var userConnection = new UserConnection()
                    {
                        FirstAccessedDate = DateTime.Now,
                        LastAccessedDate = DateTime.Now,
                        ClientConnectionId = userConnectionId
                    };

                    await _context.AddAsync(userConnection);

                    await _context.SaveChangesAsync();

                    var userConnectionDetail = new UserConnectionDetail()
                    {
                        DateViewed = DateTime.Now,
                        ProductViewed = productId,
                        UserConnectionId = userConnection.UserConnectionId
                    };

                    await _context.AddAsync(userConnectionDetail);

                    await _context.SaveChangesAsync();

                    transaction.Complete();

                    return;
                }

                if(await IsExistsConnectionIdViewedProductAsync(userConnectionId, productId))
                {
                    return;
                }

                var userConnectionResult = await _context.UserConnections.Where(x => x.ClientConnectionId == userConnectionId).FirstOrDefaultAsync();

                var userConnectionDetailAddNew = new UserConnectionDetail()
                {
                    DateViewed = DateTime.Now,
                    ProductViewed = productId,
                    UserConnectionId = userConnectionResult.UserConnectionId
                };

                await _context.AddAsync(userConnectionDetailAddNew);

                await _context.SaveChangesAsync();

                transaction.Complete();
            }
            catch
            {
            }

        }

        public async Task<bool> IsExistsConnectionIdAsync(string connectionId)
        {
            var result = await _context.UserConnections.Where(x => x.ClientConnectionId.Equals(connectionId)).FirstOrDefaultAsync();

            if(result != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsExistsConnectionIdViewedProductAsync(string connectionId, int productId)
        {
            var result = await _context.UserConnections.Where(x => x.ClientConnectionId.Equals(connectionId)).FirstOrDefaultAsync();

            if (result == null)
            {
                return false;
            }

            if (result.UserConnectionDetails != null && result.UserConnectionDetails.Select(x => x.ProductViewed == productId).Any())
            {
                return true;
            }

            return false;
        }
    }
}
