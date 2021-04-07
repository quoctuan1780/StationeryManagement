using Common;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Services.Interfacies;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> AddRoleAsync(User user)
        {
            try
            {
                return await _userManager.AddToRoleAsync(user, RoleConstant.ROLE_CUSTOMER);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GenerateEmailConfirmTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<byte> LoginAsync(string email, string password)
        {
            var emailFinded = await _userManager.FindByEmailAsync(email);

            if (emailFinded is null) return Constant.CODE_NOT_EXISTS_ACCOUNT;

            var emailConfirmed = await _userManager.IsEmailConfirmedAsync(emailFinded);

            if (!emailConfirmed) return Constant.ERROR_CODE_DO_NOT_CONFIRM_EMAIL;

            var result = await _signInManager.PasswordSignInAsync(email, password, true, true);

            if (result.Succeeded)
            {
                return Constant.CODE_SUCCESS;
            }

            if (result.IsLockedOut)
            {
                return Constant.CODE_LOOK_ACCOUNT;
            }

            return Constant.CODE_FAIL;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> RegisterAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
    }
}
