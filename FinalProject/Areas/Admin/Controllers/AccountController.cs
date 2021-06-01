using FinalProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Threading.Tasks;
using static Common.Constant;
using static Common.MessageConstant;
using static Common.RoleConstant;
﻿using Entities.Models;
using System.Transactions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication;
using FinalProject.Heplers;
using FinalProject.Areas.Admin.Helpers;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
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
       
        public IActionResult Index()
        {
            ViewBag.Users = _accountService.GetAllEmployeesAync(); 
            return View();
        }
        public async Task<IActionResult> CreateEmployeeAccount()
        {
            ViewBag.Provinces = await _addressService.GetProvincesAsync();
            ViewBag.Role = await _accountService.GetUserRole();

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
                        if (await _accountService.IsInRoleAsync(user, ROLE_ADMIN))
                        {
                            await SecurityManager.SignInAsync(HttpContext, user, ROLE_ADMIN, ROLE_ADMIN);
                        }
                        return Redirect("/Admin/Home/Dashboard");

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

        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();

            await HttpContext.SignOutAsync(scheme: ROLE_ADMIN);

            return Redirect("/Admin/Account/Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployeeAccount(CreateAccountEmployeeViewModel model)
        {
            try
            {
                var fileName = await ProductHelper.SaveImageAccountAsync(model.Image, 1920, 1080, model.Email);

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
                };
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                var result = await _accountService.CreateAccountAsync(user, model.Role);
                if (result.Succeeded)
                {
                    transaction.Complete();

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
    }
}
