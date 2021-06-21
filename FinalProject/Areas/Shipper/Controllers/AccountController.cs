using Entities.Models;
using FinalProject.Areas.Admin.Helpers;
using FinalProject.Areas.Shipper.ViewModels;
using FinalProject.Heplers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfacies;
using System;
using System.Threading.Tasks;
using System.Transactions;
using static Common.Constant;
using static Common.MessageConstant;
using static Common.RoleConstant;

namespace FinalProject.Areas.Shipper.Controllers
{
    [Area(ROLE_SHIPPER)]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAddressService _addressService;

        public AccountController(IAccountService accountService, IAddressService addressService)
        {
            _accountService = accountService;
            _addressService = addressService;
        }

        
        public IActionResult Login()
        {
            return View();
        }

        [Authorize(Roles = ROLE_SHIPPER, AuthenticationSchemes = ROLE_SHIPPER)]
        public IActionResult ChangePassword()
        {
            return View();
        }

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


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = ROLE_SHIPPER, AuthenticationSchemes = ROLE_SHIPPER)]
        //public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _accountService.GetUserAsync(User);
        //        var result = await _accountService.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        //        if (result.Succeeded)
        //        {
        //            ViewBag.MessageSuccess = "Đổi mật khẩu thành công";
        //        }
        //        else
        //        {
        //            ViewBag.MessageFail = "Mật khẩu cũ không chính xác";
        //        }
        //    }

        //    return View(model);
        //}

        [Authorize(Roles = ROLE_SHIPPER, AuthenticationSchemes = ROLE_SHIPPER)]
        public async Task<IActionResult> Information()
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

                    var model = AccountHelper.ConvertFromUserToInformationClientViewModel(user);

                    return View(model);
                }
            }
            catch
            {
            }
            return PartialView(ERROR_404_PAGE);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ROLE_SHIPPER, AuthenticationSchemes = ROLE_SHIPPER)]
        public async Task<IActionResult> Information(InformationViewModel model)
        {

            ViewBag.Provinces = await _addressService.GetProvincesAsync();

            ViewBag.Districts = await _addressService.GetDistrictsByProvinceIdAsync(model.ProvinceId);

            ViewBag.Wards = await _addressService.GetWardsByDistrictIdAsync(model.DistrictId);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                User user = null;
                if (!(model.Image is null))
                {
                    var saveAvatarResult = await Heplers.ImageHelper.SaveImageAsync(model.Image, 300, 300, model.Email);

                    user = new User()
                    {
                        Id = model.UserId,
                        PhoneNumber = model.PhoneNumber,
                        Gender = model.Gender,
                        DateOfBirth = model.DateOfBirth,
                        Image = saveAvatarResult,
                        WardCode = model.WardCode,
                        StreetName = model.StreetName,
                        FullName = model.FullName,
                        ModifyDate = DateTime.Now
                    };

                    model.ImageLink = saveAvatarResult;
                }
                else
                {
                    user = new User()
                    {
                        Id = model.UserId,
                        PhoneNumber = model.PhoneNumber,
                        Gender = model.Gender,
                        DateOfBirth = model.DateOfBirth,
                        WardCode = model.WardCode,
                        StreetName = model.StreetName,
                        FullName = model.FullName,
                        ModifyDate = DateTime.Now
                    };
                }

                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                var result = await _accountService.UpdateInformationClientAsync(user);

                if (result > 0)
                {
                    transaction.Complete();

                    ViewBag.MessageSuccess = MESSAGE_SUCCESS_UPDATE_ACCOUNT_INFOR;
                }

                else throw new Exception();
            }
            catch
            {
                ViewBag.MessageDanger = MESSAGE_ERROR_UPDATE_ACCOUNT_INFOR;
                return View(model);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Admin.ViewModels.LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.LoginAsync(model.Email, model.Password);

                switch (result)
                {
                    case CODE_SUCCESS:
                        var user = await _accountService.GetUserByEmailAsync(model.Email);
                        if (await _accountService.IsInRoleAsync(user, ROLE_SHIPPER))
                        {
                            await SecurityManager.SignInAsync(HttpContext, user, ROLE_SHIPPER, ROLE_SHIPPER);
                        }
                        return Redirect("/Shipper/Home/Dashboard");

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

        public async Task<string> District(int? provinceId)
        {
            if (provinceId is null)
            {
                return null;
            }

            var result = await _addressService.GetDistrictsByProvinceIdAsync(provinceId.Value);

            return JsonConvert.SerializeObject(result); ;
        }


        public async Task<string> Ward(int? districtId)
        {
            if (districtId is null)
            {
                return null;
            }

            var result = await _addressService.GetWardsByDistrictIdAsync(districtId.Value);

            return JsonConvert.SerializeObject(result);
        }

        [Authorize(Roles = ROLE_SHIPPER, AuthenticationSchemes = ROLE_SHIPPER)]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();

            await HttpContext.SignOutAsync(scheme: ROLE_SHIPPER);

            return Redirect("/Shipper/Account/Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = ROLE_SHIPPER, AuthenticationSchemes = ROLE_SHIPPER)]
        public async Task<IActionResult> ViewInfor(CreateAccountEmployeeViewModel model)
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

    }
}
