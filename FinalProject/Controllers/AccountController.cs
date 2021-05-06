using Common;
using Entities.Models;
using FinalProject.Heplers;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfacies;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace FinalProject.Controllers
{
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
        public IActionResult Login(string urlBack)
        {
            ViewBag.UrlBack = urlBack;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string urlBack)
        {
            urlBack ??= Url.Content(Constant.ROUTE_HOME_INDEX_CLIENT);


            if (ModelState.IsValid)
            {
                var result = await _accountService.LoginAsync(model.Email, model.Password);

                switch (result)
                {
                    case Constant.CODE_SUCCESS:
                        return Redirect(urlBack);

                    case Constant.CODE_FAIL:
                        ViewBag.Message = MessageConstant.MESSAGE_ERROR_LOGIN_WRONG;
                        break;

                    case Constant.CODE_NOT_EXISTS_ACCOUNT:
                        ViewBag.Message = MessageConstant.MESSAGE_ERROR_EXISTS_ACCOUNT;
                        break;

                    case Constant.CODE_LOOK_ACCOUNT:
                        ViewBag.Message = MessageConstant.MESSAGE_ERROR_LOCKED_ACCOUNT;
                        break;

                    case Constant.ERROR_CODE_DO_NOT_CONFIRM_EMAIL:
                        ViewBag.Message = MessageConstant.MESSAGE_ERROR_CONFIRM_EMAIL;

                        break;
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Register(string urlBack)
        {
            ViewBag.UrlBack = urlBack;

            ViewBag.Provinces = await _addressService.GetProvincesAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string urlBack)
        {
            ViewBag.UrlBack = urlBack;

            ViewBag.Provinces = await _addressService.GetProvincesAsync();

            if (ModelState.IsValid)
            {

                urlBack ??= Url.Content(Constant.ROUTE_LOGIN_CLIENT);

                var user = new User()
                {
                    FullName = model.FullName,
                    Gender = model.Gender,
                    UserName = model.Email,
                    Email = model.Email,
                    DateOfBirth = model.DateOfBirth,
                    PhoneNumber = model.PhoneNumber,
                    WardCode = model.WardCode,
                    StreetName = model.StreetName
                };

                var result = await _accountService.RegisterAsync(user, model.Password);

                if(!result.Succeeded)
                {
                    ViewBag.Message = MessageConstant.MESSAGE_ERROR_ACCOUNT_REGISTER;

                    return View(model);
                }

                var resultAddRole = await _accountService.AddRoleAsync(user);

                if (resultAddRole is null)
                {
                    ViewBag.Message = MessageConstant.MESSAGE_ERROR_STRONG_PASSWORD;
                    
                    return View(model);
                }

                var token = await _accountService.GenerateEmailConfirmTokenAsync(user);

                var callbackUrl = 
                    Url.ActionLink(Constant.ACTION_CONFIRM_EMAIL, Constant.CONTROLLER_ACCOUNT, 
                        new { UserId = user.Id, Token = token }, 
                        Request.Scheme);

                string subject = Constant.EMAIL_SUBJECT;

                string body = Constant.EMAIL_HEADER_START + user.Email 
                    + Constant.EMAIL_HEADER_END + Constant.EMAIL_BODY_START + callbackUrl + Constant.EMAIL_BODY_END;

                await _emailSender.SendEmailAsync(user.Email, subject, body);

                if (resultAddRole.Succeeded)
                {
                    await _accountService.LoginAsync(model.Email, model.Password);

                    TempData[Constant.KEY_CONFIRM_EMAIL] = MessageConstant.MESSAGE_CONFIRM_EMAIL_REGISTER;

                    return Redirect(urlBack);
                }
                else
                {
                    ViewBag.Message = MessageConstant.MESSAGE_ERROR_SYSTEM;
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _accountService.ConfirmEmailAsync(userId, token);

            if (result.Succeeded)
            {
                TempData[Constant.KEY_CONFIRM_EMAIL_SUCCESS] = MessageConstant.MESSAGE_CONFIRM_EMAIL_SUCCESS;

                return View(Constant.VIEW_LOGIN);
            }
            else
            {
                return Redirect(Constant.ROUTE_REGISTER_CLIENT);
            }
        }

        public async Task<string> District(int? provinceId)
        {
            if(provinceId is null)
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

        public async Task<IActionResult> Logout(string urlBack = null)
        {
            await _accountService.LogoutAsync();

            urlBack ??= Url.Content(Constant.ROUTE_HOME_INDEX_CLIENT);

            return Redirect(urlBack);
        }

        public async Task<IActionResult> Information()
        {
            var userId = _accountService.GetUserId(User);

            if(!(userId is null))
            {
                var user = await _accountService.GetUserByUserIdAsync(userId);

                ViewBag.Provinces = await _addressService.GetProvincesAsync();

                if(!(user is null) && !(user.WardCode is null))
                {
                    ViewBag.Districts = await _addressService.GetDistrictsByProvinceIdAsync(user.Ward.District.Province.ProvinceId);

                    ViewBag.Wards = await _addressService.GetWardsByDistrictIdAsync(user.Ward.District.DistrictId);
                }

                var model = AccountHelper.ConvertFromUserToInformationClientViewModel(user);

                return View(model);
            }

            return PartialView(Constant.ERROR_404_PAGE);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Information(InformationClientViewModel model)
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
                    var saveAvatarResult = await ImageHelper.SaveImageAsync(model.Image, 300, 300, model.Email);

                    user = new User()
                    {
                        Id = model.UserId,
                        PhoneNumber = model.PhoneNumber,
                        Gender = model.Gender,
                        DateOfBirth = model.DateOfBirth,
                        Image = saveAvatarResult,
                        WardCode = model.WardCode,
                        StreetName = model.StreetName,
                        FullName = model.FullName
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
                        FullName = model.FullName
                    };
                }

                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                var result = await _accountService.UpdateInformationClientAsync(user);

                if (result > 0)
                {
                    transaction.Complete();

                    ViewBag.MessageSuccess = MessageConstant.MESSAGE_SUCCESS_UPDATE_ACCOUNT_INFOR;
                }

                else throw new Exception();
            }
            catch
            {
                ViewBag.MessageDanger = MessageConstant.MESSAGE_ERROR_UPDATE_ACCOUNT_INFOR;
                return View(model);
            }
            return View(model);
        }
    }
}
