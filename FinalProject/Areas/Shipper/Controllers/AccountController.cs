using Entities.Models;
using FinalProject.Areas.Shipper.Helpers;
using FinalProject.Areas.Shipper.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Services.Interfacies;
using System;
using System.Text;
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
        private readonly IEmailSender _emailSender;

        public AccountController(IAccountService accountService, IAddressService addressService, IEmailSender emailSender)
        {
            _accountService = accountService;
            _addressService = addressService;
            _emailSender = emailSender;
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
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ROLE_SHIPPER, AuthenticationSchemes = ROLE_SHIPPER)]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountService.GetUserAsync(User);

                var result = await _accountService.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    ViewBag.MessageSuccess = "Đổi mật khẩu thành công";
                }
                else
                {
                    ViewBag.MessageFail = "Mật khẩu cũ không chính xác";
                }
            }

            return View(model);
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
        public async Task<IActionResult> Login(LoginViewModel model, string urlBack)
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
                            await Heplers.SecurityManager.SignInAsync(HttpContext, user, ROLE_SHIPPER, ROLE_SHIPPER);
                        }
                        if(urlBack is not null || !urlBack.Equals(EMPTY))
                        {
                            return Redirect(urlBack);
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

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (email is null)
            {
                ViewBag.Message = "Địa chỉ email không được để trống";
                return View();
            }

            var user = await _accountService.GetUserByEmailAsync(email);

            if (user is null)
            {
                ViewBag.Message = "Tài khoản không tồn tại trong hệ thống";
                return View();
            }

            bool checkRoleWarehouse = await _accountService.IsInRoleAsync(user, ROLE_SHIPPER);

            if (!checkRoleWarehouse)
            {
                ViewBag.Message = "Tài khoản này không phải tài khoản của người giao hàng";
                return View();
            }

            var token = await _accountService.GeneratePasswordResetTokenAsync(user);
            var tokenEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var callbackUrl =
                        Url.ActionLink("ConfirmForgotPassword", CONTROLLER_ACCOUNT,
                            new { Area = AREA_SHIPPER, Email = email, Token = tokenEncoded },
                            Request.Scheme);

            var subject = "Quên mật khẩu!";

            var body = "<h2>Xin chào bạn: " + email + "</h2>" + "<p>Bạn đã có một yêu cầu khôi phục mật khẩu \n " +
                "vui lòng nhấn vào link sau để khôi phục mật khẩu của bạn</p>" +
                "<a href='" + callbackUrl + "'>Nhấn vào đây</a>";

            await _emailSender.SendEmailAsync(email, subject, body);
            ViewBag.MessageSuccess = "Vui lòng mở email để khôi phục mật khẩu";
            return View();
        }

        public IActionResult ConfirmForgotPassword(string email, string token)
        {
            var model = new ForgotPasswordViewModel()
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountService.GetUserByEmailAsync(model.Email);

                var tokenDecoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));

                var result = await _accountService.ForgotPasswordAsync(user, tokenDecoded, model.Password);

                if (result.Succeeded)
                {
                    TempData[KEY_CONFIRM_EMAIL_SUCCESS] = "Khôi phục mật khẩu thành công";

                    return Redirect("/Shipper/Account/Login");
                }

                ViewBag.Message = "Token đã hết hạn";
            }

            return View(model);
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
                var fileName = await Helpers.ImageHelper.SaveImageAsync(model.Image, 1920, 1080, model.Email);

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
