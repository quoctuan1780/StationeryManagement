using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Common.RoleConstant;

namespace FinalProject.Areas.Shipper.Controllers
{
    [Area("Shipper")]
    [Authorize(Roles = ROLE_SHIPPER)]
    public class HomeController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
