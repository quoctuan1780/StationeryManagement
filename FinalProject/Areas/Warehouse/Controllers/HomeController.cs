using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Common.Constant;
using static Common.RoleConstant;

namespace FinalProject.Areas.Warehouse.Controllers
{
    [Area(AREA_WAREHOUSE)]
    [Authorize(Roles = ROLE_WAREHOUSE_MANAGER, AuthenticationSchemes = ROLE_WAREHOUSE_MANAGER)]
    public class HomeController : Controller
    {

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
