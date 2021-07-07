using Entities.Models;
using FinalProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services.Interfacies;
using System.Threading.Tasks;
using System.Transactions;
using static Common.Constant;
using static Common.RoleConstant;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
    [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
    public class GuideController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IFileGuideService _fileGuideService;
        private readonly IMemoryCache _memoryCache;

        public GuideController(IAccountService accountService, IFileGuideService fileGuideService, IMemoryCache memoryCache)
        {
            _accountService = accountService;
            _fileGuideService = fileGuideService;
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> UploadGuideBuyProduct(UploadViewModel model)
        {
            var user = await _accountService.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                try
                {
                    var result = await _fileGuideService.AddOrUpdateFilePdfAsync(model.File, user.Id, GUIDE_BUY_PRODUCT);

                    if (result > 0)
                    {
                        transaction.Complete();

                        bool checkExistsCache = _memoryCache.TryGetValue(GUIDE_BUY_PRODUCT, out FileGuide fileGuide);

                        if (checkExistsCache)
                        {
                            _memoryCache.Remove(GUIDE_BUY_PRODUCT);
                        }

                        ViewBag.Message = "Thêm hướng dẫn thành công";

                        return View("Index", model);
                    }
                }
                catch
                {

                }
            }
            ViewBag.MessageEror = "Thêm hướng dẫn không thành công";
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> UploadGuidePayment(UploadViewModel model)
        {
            var user = await _accountService.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                try
                {
                    var result = await _fileGuideService.AddOrUpdateFilePdfAsync(model.File, user.Id, GUIDE_PAYMENT);

                    if (result > 0)
                    {
                        transaction.Complete();

                        bool checkExistsCache = _memoryCache.TryGetValue(GUIDE_PAYMENT, out FileGuide fileGuide);

                        if (checkExistsCache)
                        {
                            _memoryCache.Remove(GUIDE_PAYMENT);
                        }

                        ViewBag.MessageAddPayment = "Thêm hướng dẫn thành công";

                        return View("Index", model);
                    }
                }
                catch
                {

                }
            }
            ViewBag.MessageErorAddPayment = "Thêm hướng dẫn không thành công";
            return View("Index", model);
        }
    }
}
