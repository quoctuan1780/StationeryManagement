using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    }
}
