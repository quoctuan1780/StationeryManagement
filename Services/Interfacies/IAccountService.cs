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

        Task<int> UpdateInformationClientAsync(User user);

        Task<IList<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();

        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirecuUrl);

        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();

        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor);

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<IdentityResult> CreateAccountAsync(User user, string role);
        public Task<IdentityResult> AddRoleAsync(User user, string role);

        public Task<IList<IdentityUser>> GetAllEmployeesAync();
        public Task<int> CountAccountContainsTextAsync(string text);

        public Task<List<IdentityRole>> GetUserRole();
    }
}
