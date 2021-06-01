using Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IAccountService
    {
        Task<byte> LoginAsync(string email, string password);
        Task LoginAsync(User user, bool isPersistent);
        Task<IdentityResult> RegisterAsync(User user, string password);
        Task<IdentityResult> RegisterAsync(User user, ExternalLoginInfo info);
        Task<IdentityResult> AddRoleAsync(User user);
        Task<string> GenerateEmailConfirmTokenAsync(User user);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
        Task LogoutAsync();
        string GetUserId(ClaimsPrincipal user);
        Task<User> GetUserAsync(ClaimsPrincipal user);
        Task<User> GetUserByUserIdAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<int> UpdateInformationClientAsync(User user);
        Task<IList<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirecuUrl);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task<IdentityResult> CreateAccountAsync(User user, string role);
        Task<IdentityResult> AddRoleAsync(User user, string role);
        Task<IList<User>> GetAllEmployeesAync();
        Task<int> CountAccountContainsTextAsync(string text);
        Task<List<IdentityRole>> GetUserRole();
        Task<bool> IsInRoleAsync(User user, string role);
    }
}
