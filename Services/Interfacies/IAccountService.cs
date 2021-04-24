using Entities.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IAccountService
    {
        Task<byte> LoginAsync(string email, string password);

        Task<IdentityResult> RegisterAsync(User user, string password);

        Task<IdentityResult> AddRoleAsync(User user);

        Task<string> GenerateEmailConfirmTokenAsync(User user);

        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);

        Task LogoutAsync();

        string GetUserId(ClaimsPrincipal user);

        Task<User> GetUserAsync(ClaimsPrincipal user);

        Task<User> GetUserByUserIdAsync(string userId);

        Task<int> UpdateInformationClientAsync(User user);
    }
}
