using FinalProject.Areas.Warehouse.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Threading.Tasks;
using static Common.Constant;
using static Common.RoleConstant;
using static Common.MessageConstant;
using FinalProject.Heplers;
using Microsoft.AspNetCore.Authentication;

namespace FinalProject.Areas.Warehouse.Controllers
{
    [Area(AREA_WAREHOUSE)]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public IActionResult Login()
        {
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
                        if (await _accountService.IsInRoleAsync(user, ROLE_WAREHOUSE_MANAGER))
                        {
                            await SecurityManager.SignInAsync(HttpContext, user, ROLE_WAREHOUSE_MANAGER, ROLE_WAREHOUSE_MANAGER);
                        }
                        return Redirect("/Warehouse/Home/Dashboard");

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

            await HttpContext.SignOutAsync(scheme: ROLE_WAREHOUSE_MANAGER);

            return Redirect("/Warehouse/Account/Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
