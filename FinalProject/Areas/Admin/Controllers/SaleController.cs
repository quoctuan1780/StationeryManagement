using static Common.Constant;
using static Common.RoleConstant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
    [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
    public class SaleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
