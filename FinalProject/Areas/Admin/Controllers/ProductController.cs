using Common;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(RoleConstant.ROLE_ADMIN)]
    public class ProductController : Controller
    {
        public IActionResult AddProduct()
        {
            return View();
        }
    }
}
