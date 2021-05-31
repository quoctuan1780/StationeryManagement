using FinalProject.Areas.Shipper.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Threading.Tasks;
using static Common.Constant;
using static Common.MessageConstant;
using static Common.RoleConstant;

namespace FinalProject.Areas.Shipper.Controllers
{
    [Area("Shipper")]
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
                        if (User.IsInRole(ROLE_ADMIN))
                        {
                            return Redirect("/Admin/Home/Dashboard");
                        }
                        else if (User.IsInRole(ROLE_SHIPPER))
                        {
                            return Redirect("/Shipper/Home/Dashboard");
                        }
                        break;

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
    }
}
