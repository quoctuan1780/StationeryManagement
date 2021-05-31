using static Common.Constant;
using static Common.RoleConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
    [Authorize(Roles = ROLE_ADMIN)]
    public class HomeController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
