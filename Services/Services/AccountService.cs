using Common;
using Entities.Data;
using Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ShopDbContext _context;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, ShopDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<IdentityResult> AddRoleAsync(User user)
        {
            return await _userManager.AddToRoleAsync(user, RoleConstant.ROLE_CUSTOMER);
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

        public Task<string> GetUserIdAsync(Microsoft.AspNetCore.Mvc.ControllerBase User)
        {
            throw new System.NotImplementedException();
        }

        public string GetUserId(ClaimsPrincipal user)
        {
            return _userManager.GetUserId(user);
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

        public async Task<User> GetUserAsync(ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }

        public async Task<User> GetUserByUserIdAsync(string userId)
        {
            return await _context.Users.Where(x => x.Id == userId)
                                       .Include(x => x.Ward)
                                       .ThenInclude(x => x.District)
                                       .ThenInclude(x => x.Province)
                                       .FirstOrDefaultAsync();
        }

        public async Task<int> UpdateInformationClientAsync(User user)
        {
            if (user is null) return 0;

            var userDb = await _context.Users.FirstAsync(x => x.Id == user.Id);

            if(!(user.Image is null))
            {
                userDb.Image = user.Image;
            }

            userDb.FullName = user.FullName;
            userDb.Gender = user.Gender;
            userDb.PhoneNumber = user.PhoneNumber;
            userDb.WardCode = user.WardCode;
            userDb.StreetName = user.StreetName;
            userDb.DateOfBirth = user.DateOfBirth;

            _context.Users.Update(userDb);

            return await _context.SaveChangesAsync();
        }

        public async Task<IList<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
        {
            return (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirecuUrl)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, redirecuUrl);
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return await _signInManager.GetExternalLoginInfoAsync();
        }

        public async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor)
        {
            return await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<IdentityResult> RegisterAsync(User user, ExternalLoginInfo info)
        {
            await _userManager.CreateAsync(user);

            return await _userManager.AddLoginAsync(user, info);
        }

        public async Task LoginAsync(User user, bool isPersistent)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
        }
    }
}
