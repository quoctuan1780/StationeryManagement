using Common;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfacies;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAddressService _addressService;

        public AccountController(IAccountService accountService, IAddressService addressService)
        {
            _accountService = accountService;
            _addressService = addressService;
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
            if (ModelState.IsValid)
            {
                var result = await _accountService.LoginAsync(model.Email, model.Password);

                switch (result)
                {
                    case Constant.CODE_SUCCESS:
                        return Redirect(urlBack);

                    case Constant.CODE_FAIL:
                        break;

                    case Constant.CODE_NOT_EXISTS_ACCOUNT:
                        break;

                    case Constant.CODE_LOOK_ACCOUNT:
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
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            return View();
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
    }
}
