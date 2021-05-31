using Entities.Models;
using FinalProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfacies;
using System.Threading.Tasks;
using System.Transactions;
using static Common.Constant;

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
