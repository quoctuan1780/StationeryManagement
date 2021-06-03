using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Common.RoleConstant;
using static Common.Constant;

namespace FinalProject.Areas.Shipper.Controllers
{
    [Area(AREA_SHIPPER)]
    [Authorize(Roles = ROLE_SHIPPER, AuthenticationSchemes = ROLE_SHIPPER)]
    public class HomeController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
