using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Threading.Tasks;
using static Common.RoleConstant;
using static Common.Constant;
using X.PagedList;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ISaleService _saleService;
        private readonly IAccountService _accountService;
        private readonly INotificationService _notificationService;
        public IRecommendationService _recommandationService;

        public HomeController(IProductService productService, IRecommendationService recommendationService, ISaleService saleService, IAccountService accountService, INotificationService notificationService)
        {
            _productService = productService;
            _recommandationService = recommendationService;
            _saleService = saleService;
            _accountService = accountService;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Products = await _productService.GetAllProductsAsync();

            ViewBag.Sales = await _saleService.GetThreeSalesImageAsync();

            ViewBag.Top10ProductsHot = await _productService.GetTop10ProductHotAsync();

            return View();
        }

        public async Task<IActionResult> SaleProduct()
        {
            ViewBag.Products = await _productService.GetProductsSaleAsync();

            return View();
        }

        [Authorize(Roles = ROLE_CUSTOMER, AuthenticationSchemes = ROLE_CUSTOMER)]
        public async Task<IActionResult> Notifications(int? page = 1)
        {
            var user = await _accountService.GetUserAsync(User);

            ViewBag.User = user;

            ViewBag.Products = await _productService.GetProductsSaleAsync();

            var result = await _notificationService.GetNotificationsAsync(ROLE_CUSTOMER ,user.Id);

            var model = result.ToPagedList(page.Value, 10);

            return View(model);
        }

        [HttpDelete]
        public async Task<int> DeleteNofity(int? notificationId)
        {
            if (notificationId is null)
            {
                return ERROR_CODE_NULL;
            }

            var result = await _notificationService.DeleteStatusAsync(notificationId.Value);

            if (result > 0)
            {
                return CODE_SUCCESS;
            }

            return ERROR_CODE_SYSTEM;
        }

        [HttpPut]
        public async Task<int> SeenNotify(int? notificationId)
        {
            if (notificationId is null)
            {
                return ERROR_CODE_NULL;
            }

            var result = await _notificationService.UpdateStatusAsync(notificationId.Value);

            if (result > 0)
            {
                return CODE_SUCCESS;
            }

            return ERROR_CODE_SYSTEM;
        }
    }
}
