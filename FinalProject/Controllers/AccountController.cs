using Entities.Models;
using FinalProject.Heplers;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfacies;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using static Common.Constant;
using static Common.MessageConstant;
using static Common.RoleConstant;

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
        public async Task<IActionResult> Login(string urlBack)
        {
            ViewBag.UrlBack = urlBack;

            var model = new LoginViewModel()
            {
                ReturnUrl = urlBack,
                ExternalLogins = await _accountService.GetExternalAuthenticationSchemesAsync()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action(VIEW_EXTERNAL_LOGIN, CONTROLLER_ACCOUNT, new { ReturnUrl = returnUrl });

            var properties = _accountService.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            returnUrl ??= Url.Content(ROUTE_HOME_INDEX_CLIENT);

            var loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _accountService.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            var info = await _accountService.GetExternalLoginInfoAsync();

            if (info == null)
            {
                ModelState.AddModelError(EMPTY, MESSAGE_ERROR_GET_EXTERNAL_INFOMATION_ACCOUNT);

                return View(VIEW_LOGIN, loginViewModel);
            }

            var signInResult = await _accountService.ExternalLoginSignInAsync(info.LoginProvider,
                                            info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                if (returnUrl.Equals(ROUTE_LOGIN_CLIENT))
                {
                    return Redirect(ROUTE_HOME_INDEX_CLIENT);
                }
                else
                {
                    return LocalRedirect(returnUrl);
                }
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                ViewBag.UrlBack = returnUrl;

                ViewBag.Provinces = await _addressService.GetProvincesAsync();

                var model = new RegisterViewModel()
                {
                    Email = email,
                    ReturnUrl = returnUrl,
                    ExternalLogins = (await _accountService.GetExternalAuthenticationSchemesAsync()).ToList()
                };

                if (info.ProviderDisplayName.Equals(PROVIDER_GOOGLE))
                {
                    return View(VIEW_REGISTER_WITH_GOOGLE, model);
                }
                else if (info.ProviderDisplayName.Equals(PROVIDER_FACEBOOK))
                {
                    return View(VIEW_REGISTER_WITH_FACEBOOK, model);
                }
                else
                {
                    return Redirect(ROUTE_LOGIN_CLIENT);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string urlBack)
        {
            urlBack ??= Url.Content(ROUTE_HOME_INDEX_CLIENT);


            if (ModelState.IsValid)
            {
                var result = await _accountService.LoginAsync(model.Email, model.Password);

                switch (result)
                {
                    case CODE_SUCCESS:
                        return Redirect(urlBack);

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

        public async Task<IActionResult> Register(string urlBack)
        {
            ViewBag.UrlBack = urlBack;

            ViewBag.Provinces = await _addressService.GetProvincesAsync();

            var model = new RegisterViewModel()
            {
                ReturnUrl = urlBack,
                ExternalLogins = await _accountService.GetExternalAuthenticationSchemesAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string urlBack)
        {
            ViewBag.UrlBack = urlBack;

            ViewBag.Provinces = await _addressService.GetProvincesAsync();

            if (ModelState.IsValid)
            {
                urlBack ??= Url.Content(ROUTE_LOGIN_CLIENT);

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
                using var trabsaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                try
                {
                    var result = await _accountService.RegisterAsync(user, model.Password);

                    if (!result.Succeeded)
                    {
                        ViewBag.Message = MESSAGE_ERROR_ACCOUNT_REGISTER;

                        return View(model);
                    }

                    var resultAddRole = await _accountService.AddRoleAsync(user);

                    if (resultAddRole is null)
                    {
                        ViewBag.Message = MESSAGE_ERROR_STRONG_PASSWORD;

                        return View(model);
                    }

                    var token = await _accountService.GenerateEmailConfirmTokenAsync(user);

                    var callbackUrl =
                        Url.ActionLink(ACTION_CONFIRM_EMAIL, CONTROLLER_ACCOUNT,
                            new { UserId = user.Id, Token = token },
                            Request.Scheme);

                    string subject = EMAIL_SUBJECT;

                    string body = EMAIL_HEADER_START + user.Email
                        + EMAIL_HEADER_END + EMAIL_BODY_START + callbackUrl + EMAIL_BODY_END;

                    await _emailSender.SendEmailAsync(user.Email, subject, body);

                    if (resultAddRole.Succeeded)
                    {
                        await _accountService.LoginAsync(model.Email, model.Password);

                        TempData[KEY_CONFIRM_EMAIL] = MESSAGE_CONFIRM_EMAIL_REGISTER;

                        trabsaction.Complete();

                        return Redirect(urlBack);
                    }
                    else
                    {
                        ViewBag.Message = MESSAGE_ERROR_SYSTEM;
                    }
                }
                catch
                {
                    ViewBag.Message = MESSAGE_ERROR_SYSTEM;
                }
            }

            return View(model);
        }

        #region Register With External Account

        [HttpPost]
        public async Task<IActionResult> RegisterWithGoogle(RegisterViewModel model)
        {
            ViewBag.UrlBack = model.ReturnUrl;
            ViewBag.Provinces = await _addressService.GetProvincesAsync();

            ModelState.Remove(MODEL_FIELD_PASSWORD);
            ModelState.Remove(MODEL_FIELD_CONFIRM_PASSWORD);

            if (!ModelState.IsValid)
            {
                return View(VIEW_REGISTER_WITH_GOOGLE, model);
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                model.ReturnUrl ??= Url.Content(ROUTE_LOGIN_CLIENT);
                var info = await _accountService.GetExternalLoginInfoAsync();
                var user = new User()
                {
                    FullName = model.FullName,
                    Gender = model.Gender,
                    UserName = model.Email,
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                    DateOfBirth = model.DateOfBirth,
                    PhoneNumber = model.PhoneNumber,
                    WardCode = model.WardCode,
                    StreetName = model.StreetName
                };

                var result = await _accountService.RegisterAsync(user, info);

                if (!result.Succeeded)
                {
                    ViewBag.Message = MESSAGE_ERROR_ACCOUNT_REGISTER;

                    return View(VIEW_REGISTER_WITH_GOOGLE, model);
                }

                var resultAddRole = await _accountService.AddRoleAsync(user);

                if (resultAddRole is null)
                {
                    ViewBag.Message = MESSAGE_ERROR_STRONG_PASSWORD;

                    return View(VIEW_REGISTER_WITH_GOOGLE, model);
                }

                var token = await _accountService.GenerateEmailConfirmTokenAsync(user);

                await _accountService.ConfirmEmailAsync(user, token);

                if (resultAddRole.Succeeded)
                {
                    await _accountService.LoginAsync(user, false);

                    transaction.Complete();

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    ViewBag.Message = MESSAGE_ERROR_SYSTEM;

                    return View(VIEW_REGISTER_WITH_GOOGLE, model);
                }
            }
            catch
            {
                // tạm thời
                return PartialView(ERROR_404_PAGE);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegisterWithFacebook(RegisterViewModel model)
        {
            ViewBag.UrlBack = model.ReturnUrl;
            ViewBag.Provinces = await _addressService.GetProvincesAsync();

            ModelState.Remove(MODEL_FIELD_PASSWORD);
            ModelState.Remove(MODEL_FIELD_CONFIRM_PASSWORD);

            if (!ModelState.IsValid)
            {
                return View(VIEW_REGISTER_WITH_GOOGLE, model);
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                model.ReturnUrl ??= Url.Content(ROUTE_LOGIN_CLIENT);
                var info = await _accountService.GetExternalLoginInfoAsync();
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (!(email is null))
                {
                    model.Email = email;
                }

                var user = new User()
                {
                    FullName = model.FullName,
                    Gender = model.Gender,
                    UserName = model.Email,
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                    DateOfBirth = model.DateOfBirth,
                    PhoneNumber = model.PhoneNumber,
                    WardCode = model.WardCode,
                    StreetName = model.StreetName
                };

                var result = await _accountService.RegisterAsync(user, info);

                if (!result.Succeeded)
                {
                    ViewBag.Message = MESSAGE_ERROR_ACCOUNT_REGISTER;

                    return View(VIEW_REGISTER_WITH_GOOGLE, model);
                }

                var resultAddRole = await _accountService.AddRoleAsync(user);

                if (resultAddRole is null)
                {
                    ViewBag.Message = MESSAGE_ERROR_STRONG_PASSWORD;

                    return View(VIEW_REGISTER_WITH_GOOGLE, model);
                }

                var token = await _accountService.GenerateEmailConfirmTokenAsync(user);

                await _accountService.ConfirmEmailAsync(user, token);

                if (resultAddRole.Succeeded)
                {
                    await _accountService.LoginAsync(user, false);

                    transaction.Complete();

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    ViewBag.Message = MESSAGE_ERROR_SYSTEM;

                    return View(VIEW_REGISTER_WITH_GOOGLE, model);
                }
            }
            catch
            {
                // tạm thời
                return PartialView(ERROR_404_PAGE);
            }
        }
        #endregion
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _accountService.ConfirmEmailAsync(userId, token);

            if (result.Succeeded)
            {
                TempData[KEY_CONFIRM_EMAIL_SUCCESS] = MESSAGE_CONFIRM_EMAIL_SUCCESS;

                return View(VIEW_LOGIN);
            }
            else
            {
                return Redirect(ROUTE_REGISTER_CLIENT);
            }
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

        public async Task<IActionResult> Logout(string urlBack = null)
        {
            await _accountService.LogoutAsync();

            urlBack ??= Url.Content(ROUTE_HOME_INDEX_CLIENT);

            return Redirect(urlBack);
        }

        [Authorize(Roles = ROLE_CUSTOMER)]
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
        [Authorize(Roles = ROLE_CUSTOMER)]
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
    }
}
