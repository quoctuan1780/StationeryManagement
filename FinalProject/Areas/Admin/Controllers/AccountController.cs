using Entities.Models;
using FinalProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfacies;
using System;
using System.Threading.Tasks;
using System.Transactions;
using static Common.Constant;
using static Common.RoleConstant;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
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
        public async Task<IActionResult> Index()
        {
            ViewBag.Shippers =await _accountService.GetAllEmployeesByRoleAync(ROLE_SHIPPER);
            ViewBag.WM =await _accountService.GetAllEmployeesByRoleAync(ROLE_WAREHOUSE_MANAGER);
            return View();
        }
        public async Task<IActionResult> ViewInfor(string id)
        {
            ViewBag.Infor = await _accountService.GetUserByUserIdAsync(id);
            return View();
        }
        public async Task<IActionResult> CreateEmployeeAccount()
        {
            ViewBag.Provinces = await _addressService.GetProvincesAsync();
            ViewBag.Role = await _accountService.GetUserRole();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAccount(CreateAccountEmployeeViewModel model)
        {
            var name = model.FullName;        
            //split name with letter ' '
            string[] separateName = name.Split(' ');
            string userName = separateName[0];

            //get fisrt letter of each element except the last element
            for (int i = 1; i < separateName.Length - 1; i++)
            {
                separateName[i].ToLower();
                userName += separateName[i][0].ToString();
            }
            int count = await _accountService.CountAccountContainsTextAsync(userName);
            userName += count.ToString();
            try
                {
                var user = new User
                {
                    UserName = userName,
                    FullName = model.FullName,
                    Email = model.Email,
                    DateOfBirth = model.DateOfBirth,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    Image = model.Image,
                    WardCode = model.WardCode,
                };
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                var rd = new Random();
                var password = "ab" + rd.Next(100000, 999999);
                var result = await _accountService.CreateAccountAsync(user, model.Role,password);
                if (result.Succeeded)
                {
                    transaction.Complete();
                    string subject = "Chào mừng bạn tới với ngôi nhà chung Stationary Store!";
                    string area = "Shipper";
                    if (model.Role.Equals(ROLE_WAREHOUSE_MANAGER))
                    {
                        area = "Warehouse";
                    }

                     var callbackUrl =  Url.ActionLink("Login", "Account",
                            new { Area = area },
                            Request.Scheme);

                    string body = EMAIL_HEADER_START + user.FullName
                        + EMAIL_HEADER_END + "Chúc mừng bạn đã trở thành thành viên của gia đình Stationary Store!" +
                        "\n Đây là tài khoản và mật khẩu của bạn:" +
                        "\n Tài khoản: "+user.UserName+
                        "\n Mật khẩu: "+password+"\n Vui lòng không chia sẻ với bất cứ ai thông tin đăng nhập của bạn!"+ EMAIL_BODY_END;

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
    }
}
