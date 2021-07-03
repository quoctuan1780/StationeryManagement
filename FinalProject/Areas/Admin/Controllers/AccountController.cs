using FinalProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System;
using System.Threading.Tasks;
using static Common.Constant;
using static Common.RoleConstant;
using static Common.MessageConstant;
﻿using Entities.Models;
using System.Transactions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication;
using FinalProject.Heplers;
using FinalProject.Areas.Admin.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using static FinalProject.Areas.Admin.Helpers.ImageHelper;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IAccountService _accountService;
        private readonly IAddressService _addressService;
        private readonly IEmailSender _emailSender;

        public AccountController(IAccountService accountService, IAddressService addressService, IEmailSender emailSender,
            UserManager<User> userManager)
        {
            _userManager = userManager;
            _accountService = accountService;
            _addressService = addressService;
            _emailSender = emailSender;
        }
        public IActionResult Login()
        {
            return View();
        }
        [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
        public async Task<IActionResult> Index()
        {
            ViewBag.Shippers = await _accountService.GetAllEmployeesByRoleAync(ROLE_SHIPPER);
            ViewBag.WM = await _accountService.GetAllEmployeesByRoleAync(ROLE_WAREHOUSE_MANAGER);
            return View();
        }
        [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
        public async Task<IActionResult> ViewInfor()
        {
            string id = _userManager.GetUserId(User);
            ViewBag.Infor = await _accountService.GetUserByUserIdAsync(id);
            return View();
        }
        public async Task<IActionResult> ViewInforEmployee(string id)
        {
            ViewBag.Infor = await _accountService.GetUserByUserIdAsync(id);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
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
        [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
        public async Task<IActionResult> CreateEmployeeAccount()
        {
            ViewBag.Provinces = await _addressService.GetProvincesAsync();
            ViewBag.Role = await _accountService.GetUserRole();

            return View();
        }
        [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _accountService.GetUserAsync(User);
            var result = await _accountService.ChangePassword(user, model.CurrentPass, model.NewPass);
            if(result.Succeeded)
            {
                ViewBag.Success = "Thay đổi mật khẩu thành công!";
                return View();
            }
            ViewBag.Failed= "Thay đổi mật khẩu không thành công!";
            return View();
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
                        if (await _accountService.IsInRoleAsync(user, ROLE_ADMIN))
                        {
                            await SecurityManager.SignInAsync(HttpContext, user, ROLE_ADMIN, ROLE_ADMIN);
                        }
                        if (urlBack != null && !urlBack.Equals(EMPTY))
                        {
                            return Redirect(urlBack);
                        }
                        else
                        {
                            return Redirect("/Admin/Home/Dashboard");
                        }

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
        [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();

            await HttpContext.SignOutAsync(scheme: ROLE_ADMIN);

            return Redirect("/Admin/Account/Login");
        }


        [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployeeAccount(CreateAccountEmployeeViewModel model)
        {
            try
            {
                var fileName = await SaveImageAccountAsync(model.Image, 1920, 1080, model.Email);

                string image = "admin.png";

                if(!(fileName is null))
                {
                    image = fileName;
                }
                var user = new User
                {
                    UserName = model.Email,
                    FullName = model.FullName,
                    Email = model.Email,
                    DateOfBirth = model.DateOfBirth,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    Image = image,
                    WardCode = model.WardCode,
                    EmailConfirmed = true
                };
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            //var rd = new Random();
            //var password = "ab" + rd.Next(100000, 999999);
            var password = "Aa*123456";
                var result = await _accountService.CreateAccountAsync(user, model.Role,password);
                if (result.Succeeded)
                {
                    transaction.Complete();
                    string subject = "Chào mừng bạn tới với ngôi nhà chung Stationary Store!";
                    string area = "Warehouse";
                   

                     var callbackUrl =  Url.ActionLink("Login", "Account",
                            new { Area = area },
                            Request.Scheme);

                    string body = EMAIL_HEADER_START + user.FullName
                        + EMAIL_HEADER_END + "Chúc mừng bạn đã trở thành thành viên của gia đình Stationary Store!" +
                        "\n Đây là tài khoản và mật khẩu của bạn:" +
                        "\n Tài khoản: "+user.UserName+
                        "\n Mật khẩu: "+password+"\n Vui lòng không chia sẻ với bất cứ ai thông tin đăng nhập của bạn!";

                    await _emailSender.SendEmailAsync(user.Email, subject, body);
                    return RedirectToAction("Index");
                }

            }
            catch
            {

            }
            ViewBag.Message = "Không thể thêm mới nhân viên này";
            ViewBag.Provinces = await _addressService.GetProvincesAsync();
            ViewBag.Role = await _accountService.GetUserRole();

            return View(model);
        }

        [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
        public async Task<string> District(int? provinceId)
        {
            if (provinceId is null)
            {
                return null;
            }

            var result = await _addressService.GetDistrictsByProvinceIdAsync(provinceId.Value);

            return JsonConvert.SerializeObject(result); ;
        }

        public IActionResult AccessDenied()
        {
            return View();
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
        [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
        [HttpDelete]
        public async Task<int> Delete(string id)
        {
            if(await _accountService.DeleteUser(id) > 0)
            {
                return 1;
            }
            return 0;
        }
    }
}
