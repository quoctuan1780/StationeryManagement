using Accord.Math;
using Common;
using Entities.Data;
using Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Common.RoleConstant;

namespace Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ShopDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, ShopDbContext context,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> AddRoleAsync(User user)
        {
            return await _userManager.AddToRoleAsync(user, RoleConstant.ROLE_CUSTOMER);
        }

        public async Task<IdentityResult> AddRoleAsync(User user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            return result;
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


            var result = await _signInManager.PasswordSignInAsync(emailFinded, password, false, true);

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
            var result = await _userManager.CreateAsync(user, password);
            return result;
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

            if (!(user.Image is null))
            {
                userDb.Image = user.Image;
            }

            userDb.FullName = user.FullName;
            userDb.Gender = user.Gender;
            userDb.PhoneNumber = user.PhoneNumber;
            userDb.WardCode = user.WardCode;
            userDb.StreetName = user.StreetName;
            userDb.DateOfBirth = user.DateOfBirth;
            userDb.ModifyDate = user.ModifyDate;

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

        public async Task<IdentityResult> CreateAccountAsync(User user, string role, string password)
        {
            

            var registerResult = await RegisterAsync(user, password);
            if (registerResult.Succeeded)
            {
                return await AddRoleAsync(user, role);
            }
            return registerResult;
        }

        public async Task<List<IdentityRole>> GetUserRole()
        {
            return await _roleManager.Roles.Where(x => x.Name.Contains(ROLE_SHIPPER) || x.Name.Contains(ROLE_WAREHOUSE_MANAGER)).ToListAsync();
        }

        public async Task<int> CountAccountContainsTextAsync(string text)
        {
            var list = await _userManager.Users.Where(u => u.UserName.Contains(text)).ToListAsync();
            return list.Count;
        }

        public async Task<IList<User>> GetAllEmployeesByRoleAync(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);

            users = users.Where(x => x.IsDeleted == false).ToList();

            return users;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var users = await _userManager.FindByEmailAsync(email);
            if(users.IsDeleted == true)
            {
                return null;
            }

            return users;
        }

        public async Task<bool> IsInRoleAsync(User user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }
        public async Task<int> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);

            user.IsDeleted = true;
            _context.Users.Update(user);

            return await _context.SaveChangesAsync();
        }

        public async Task<IdentityResult> ChangePassword(User user, string currentPass, string newPass)
        {
            return await _userManager.ChangePasswordAsync(user, currentPass,newPass);
        }

        public async Task<int> UpdateInformationEmployeeAsync(User user)
        {
            var userupdate = await _context.Users.Where(x => x.UserName == user.UserName).FirstOrDefaultAsync();
            userupdate.WardCode = user.WardCode;
            userupdate.PhoneNumber = user.PhoneNumber;
            userupdate.StreetName = user.StreetName;
            userupdate.Image = user.Image;
            _context.Update(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<IList<User>> GetAllShippersAsync()
        {
            return await _userManager.GetUsersInRoleAsync(ROLE_SHIPPER);
        }

        public async Task<IList<User>> GetAllCustomersAsync()
        {
            return await _userManager.GetUsersInRoleAsync(ROLE_CUSTOMER);
        }
    }
}
