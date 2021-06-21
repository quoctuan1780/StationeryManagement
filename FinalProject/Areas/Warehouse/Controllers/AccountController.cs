using FinalProject.Areas.Warehouse.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Threading.Tasks;
using static Common.Constant;
using static Common.RoleConstant;
using static Common.MessageConstant;
using FinalProject.Heplers;
using Microsoft.AspNetCore.Authentication;
using Entities.Models;
using FinalProject.Areas.Admin.Helpers;

namespace FinalProject.Areas.Warehouse.Controllers
{
    [Area(AREA_WAREHOUSE)]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAddressService _addressService;

        public AccountController(IAccountService accountService, IAddressService addressService)
        {
            _accountService = accountService;
            _addressService = addressService;
        }

        [Authorize(Roles = ROLE_WAREHOUSE_MANAGER, AuthenticationSchemes = ROLE_WAREHOUSE_MANAGER)]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = ROLE_WAREHOUSE_MANAGER, AuthenticationSchemes = ROLE_WAREHOUSE_MANAGER)]
        public async Task<IActionResult> ViewInfor(CreateEmployeeAccountViewModel model)
        {

            try
            {
                var fileName = await ProductHelper.SaveImageAccountAsync(model.Image, 1920, 1080, model.Email);

                string image = "admin.png";

                if (!(fileName is null))
                {
                    image = fileName;
                }
                var user = new User
                {
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    WardCode = model.WardCode,
                    StreetName = model.StreetName,
                    Image = image

                };
                if (await _accountService.UpdateInformationEmployeeAsync(user) > 0)
                {
                    ViewBag.Success = "Cập nhật thông tin thành công!";
                }
                else
                {
                    ViewBag.Failed = "Cập nhật thông tin thất bại!";
                }
            }
            catch { }

            return View(model);
        }

        [Authorize(Roles = ROLE_WAREHOUSE_MANAGER, AuthenticationSchemes = ROLE_WAREHOUSE_MANAGER)]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _accountService.GetUserAsync(User);
            var result = await _accountService.ChangePassword(user, model.CurrentPass, model.NewPass);
            if (result.Succeeded)
            {
                ViewBag.Success = "Thay đổi mật khẩu thành công!";
                return View();
            }
            ViewBag.Failed = "Thay đổi mật khẩu không thành công!";
            return View();
        }

        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.LoginAsync(model.Email, model.Password);

                switch (result)
                {
                    case CODE_SUCCESS:
                        var user = await _accountService.GetUserByEmailAsync(model.Email);
                        if (await _accountService.IsInRoleAsync(user, ROLE_WAREHOUSE_MANAGER))
                        {
                            await SecurityManager.SignInAsync(HttpContext, user, ROLE_WAREHOUSE_MANAGER, ROLE_WAREHOUSE_MANAGER);
                        }
                        return Redirect("/Warehouse/Home/Dashboard");

                    case CODE_FAIL:
                        ViewBag.Message = MESSAGE_ERROR_LOGIN_WRONG;
                        break;

                    case CODE_NOT_EXISTS_ACCOUNT:
                        ViewBag.Message = MESSAGE_ERROR_EXISTS_ACCOUNT;
                        break;

                    case CODE_LOOK_ACCOUNT:
                        ViewBag.Message = MESSAGE_ERROR_LOCKED_ACCOUNT;
                        break;

                    case ERROR_CODE_DO_NOT_CONFIRM_EMAIL:
                        ViewBag.Message = MESSAGE_ERROR_CONFIRM_EMAIL;
                        break;
                }
            }

            return View(model);
        }
        [Authorize(Roles = ROLE_WAREHOUSE_MANAGER, AuthenticationSchemes = ROLE_WAREHOUSE_MANAGER)]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();

            await HttpContext.SignOutAsync(scheme: ROLE_WAREHOUSE_MANAGER);

            return Redirect("/Warehouse/Account/Login");
        }

        [Authorize(Roles = ROLE_WAREHOUSE_MANAGER, AuthenticationSchemes = ROLE_WAREHOUSE_MANAGER)]
        public async Task<IActionResult> ViewInfor(string id)
        {
            try
            {
                var userId = _accountService.GetUserId(User);

                if (!(userId is null))
                {
                    var user = await _accountService.GetUserByUserIdAsync(userId);

                    ViewBag.Provinces = await _addressService.GetProvincesAsync();

                    if (!(user is null) && !(user.WardCode is null))
                    {
                        ViewBag.Districts = await _addressService.GetDistrictsByProvinceIdAsync(user.Ward.District.Province.ProvinceId);

                        ViewBag.Wards = await _addressService.GetWardsByDistrictIdAsync(user.Ward.District.DistrictId);
                    }

                    ViewBag.User = user;

                }
            }
            catch
            {
            }
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
