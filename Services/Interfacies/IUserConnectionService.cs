using Entities.Models;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IUserConnectionService
    {
        Task<bool> IsExistsConnectionIdAsync(string connectionId);
        Task AddUserConnectionAsync(string userConnectionId, int productId);
    }
}
