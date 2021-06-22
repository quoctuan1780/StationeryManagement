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
        Task<int> UpdateInformationEmployeeAsync(User user);
        Task<IdentityResult> RegisterAsync(User user, string password);
        Task<IdentityResult> RegisterAsync(User user, ExternalLoginInfo info);
        Task<IdentityResult> AddRoleAsync(User user);
        Task<string> GenerateEmailConfirmTokenAsync(User user);
        Task<string> GeneratePasswordResetTokenAsync(User user);
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
        Task<IdentityResult> CreateAccountAsync(User user, string role,string password);
        Task<IdentityResult> AddRoleAsync(User user, string role);
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        Task<IList<User>> GetAllEmployeesByRoleAync(string role);
        Task<int> CountAccountContainsTextAsync(string text);

        Task<List<IdentityRole>> GetUserRole();
        Task<bool> IsInRoleAsync(User user, string role);
        Task<int> DeleteUser(string id);
        Task<IdentityResult> ChangePassword(User user, string currentPass, string newPass);
        Task<IList<User>> GetAllShippersAsync();
        Task<IList<User>> GetAllWarehouseManagementsAsync();
        Task<IList<User>> GetAllCustomersAsync();
        Task<IdentityResult> ForgotPasswordAsync(User user, string token, string newPassword);
    }
}
