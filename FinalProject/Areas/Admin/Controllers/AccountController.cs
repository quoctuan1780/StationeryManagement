using Entities.Models;
using FinalProject.Areas.Admin.ViewModels;
using FinalProject.Heplers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfacies;
using System;
using System.Threading.Tasks;
using System.Transactions;
using static Common.Constant;
using static Common.MessageConstant;
using static Common.RoleConstant;
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

                    var model = AccountHelper.ConvertFromUserToInformationViewModel(user);

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
        [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
        public async Task<IActionResult> Information(InformationViewModel model)
        {

            ViewBag.Provinces = await _addressService.GetProvincesAsync();
            if (model.ProvinceId != null && model.DistrictId != null)
            {
                ViewBag.Districts = await _addressService.GetDistrictsByProvinceIdAsync(model.ProvinceId.Value);
                ViewBag.Wards = await _addressService.GetWardsByDistrictIdAsync(model.DistrictId.Value);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                User user = await _accountService.GetUserAsync(User);
                user.WardCode = model.WardCode;
                user.ModifyDate = DateTime.Now;
                user.Gender = model.Gender;
                user.PhoneNumber = model.PhoneNumber;
                user.StreetName = model.StreetName;

                if (!(model.Image is null))
                {
                    var saveAvatarResult = await Heplers.ImageHelper.SaveImageAsync(model.Image, 300, 300, model.Email);

                    user.Image = saveAvatarResult;

                    model.ImageLink = saveAvatarResult;
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

        public async Task<IActionResult> ViewInforEmployee(string id)
        {
            ViewBag.Infor = await _accountService.GetUserByUserIdAsync(id);

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
            if (ModelState.IsValid)
            {
                var result = await _accountService.ChangePassword(user, model.CurrentPass, model.NewPass);
                if (result.Succeeded)
                {
                    ViewBag.Success = "Thay đổi mật khẩu thành công!";
                    return View();
                }
                ViewBag.Failed = "Thay đổi mật khẩu không thành công!";
            }
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

        [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();

            await HttpContext.SignOutAsync(scheme: ROLE_ADMIN);

            return Redirect("/Admin/Account/Login");
        }

        [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
        public async Task<IActionResult> CreateEmployeeAccount()
        {
            ViewBag.Provinces = await _addressService.GetProvincesAsync();
            ViewBag.Role = await _accountService.GetUserRole();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
        public async Task<IActionResult> CreateEmployeeAccount(CreateAccountEmployeeViewModel model)
        {
            try
            {
                var fileName = await SaveImageAccountAsync(model.Image, 1920, 1080, model.Email);

                string image = "admin.png";

                if (!(fileName is null))
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

                var rd = new Random();
                var password = "Aa*" + rd.Next(100000, 999999);

                var result = await _accountService.CreateAccountAsync(user, model.Role, password);
                if (result.Succeeded)
                {
                    transaction.Complete();
                    string subject = "Chào mừng bạn tới với ngôi nhà chung Stationary Store!";
                    string area = "Warehouse";


                    var callbackUrl = Url.ActionLink("Login", "Account",
                           new { Area = area },
                           Request.Scheme);

                    string body = EMAIL_HEADER_START + user.FullName
                        + EMAIL_HEADER_END + "Chúc mừng bạn đã trở thành thành viên của gia đình Stationary Store!" +
                        "\n Đây là tài khoản và mật khẩu của bạn:" +
                        "\n Tài khoản: " + user.UserName +
                        "\n Mật khẩu: " + password + "\n Vui lòng không chia sẻ với bất cứ ai thông tin đăng nhập của bạn!";

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

        [HttpDelete]
        [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
        public async Task<int> Delete(string id)
        {
            var result = await _accountService.DeleteUser(id);
            if (result > 0)
            {
                return CODE_SUCCESS;
            }
            return CODE_FAIL;
        }

        [HttpPut]
        [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
        public async Task<int> LockAccount(string id, bool isLocked)
        {
            var result = await _accountService.SetLockAccountAsync(id, isLocked);
            if (result != null || result.Succeeded)
            {
                return CODE_SUCCESS;
            }
            return CODE_FAIL;
        }
    }
}
