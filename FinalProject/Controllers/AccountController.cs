using Common;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
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
    }
}
