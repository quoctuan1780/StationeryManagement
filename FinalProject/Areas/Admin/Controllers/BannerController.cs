using static Common.Constant;
using static Common.RoleConstant;
using static Common.MessageConstant;
using static Common.ValidationConstant;
using static FinalProject.Areas.Admin.Helpers.BannerHelper;
using static FinalProject.Areas.Admin.Helpers.ImageHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinalProject.Areas.Admin.ViewModels;
using System.Transactions;
using Entities.Models;
using FinalProject.Areas.Admin.Helpers;
using System.Threading.Tasks;
using Services.Interfacies;
using System.Linq;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
    [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
    public class BannerController : Controller
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Banners = await _bannerService.GetBannerAsync();

            return View();
        }

        public IActionResult AddBanner()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBanner(BannerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var imageName = await SaveImageBannerAsync(model.Image, 1366, 768);

                    if (!(imageName is null))
                    {
                        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                        var banner = new Banner()
                        {
                            StartDate = model.StartDate,
                            EndDate = model.EndDate,
                            Image = imageName,
                            Url = model.Url
                        };

                        var result = await _bannerService.AddBannerAsync(banner);

                        if(result > 0)
                        {
                            transaction.Complete();

                            ViewBag.MessageSuccess = MESSAGE_SUCCESS_ADD_BANNER;

                            return View(model);
                        }
                    }
                }
                catch
                {
                }
            }

            ViewBag.MessageError = MESSAGE_ERROR_SYSTEM;

            return View(model);
        }

        [HttpDelete]
        public async Task<int> DeleteBanner(int? bannerId)
        {
            if(bannerId is null)
            {
                return ERROR_CODE_NULL;
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var result = await _bannerService.DeleteBannerAsync(bannerId.Value);

                if(result > 0)
                {
                    transaction.Complete();

                    return CODE_SUCCESS;
                }
            }
            catch { }

            return ERROR_CODE_SYSTEM;
        }

        public async Task<IActionResult> EditBanner(int? bannerId)
        {
            if(bannerId is null)
            {
                return PartialView(ERROR_404_PAGE_ADMIN);
            }

            var banner = await _bannerService.GetBannerByIdAsync(bannerId.Value);

            if(banner != null)
            {
                var model = ConvertBannerToBannerViewModel(banner);

                return View(model);
            }

            return PartialView(ERROR_404_PAGE_ADMIN);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBanner(BannerViewModel model, string imagePreviewDeteted)
        {
            ModelState.Remove(KEY_IMAGE);

            var banner = await _bannerService.GetBannerByIdAsync(model.BannerId);

            if (ModelState.IsValid)
            {
                try
                {
                    string imageName = EMPTY;

                    if (imagePreviewDeteted != null)
                    {
                        var image = imagePreviewDeteted.Split(COMMA);

                        if (image.Contains(model.Image.FileName))
                        {
                            model.Image = null;
                        }
                    }

                    if (model.Image == null && model.ImageString == null)
                    {
                        ViewBag.MessageError = ERROR_BANNER_IMAGE_EMPTY;

                        model.ImageString = banner.Image;

                        return View(model);
                    }
                    else if(model.Image != null)
                    {

                        imageName = await SaveImageBannerAsync(model.Image, 1920, 768);
                    }
                    else if(model.ImageString != null)
                    {
                        imageName = model.ImageString;
                    }

                    banner.BannerId = model.BannerId;
                    banner.StartDate = model.StartDate;
                    banner.EndDate = model.EndDate;
                    banner.Image = imageName;
                    banner.Url = model.Url;

                    using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                    var result = await _bannerService.UpdateBannerAsync(banner);

                    if(result > 0)
                    {
                        transaction.Complete();

                        ViewBag.MessageSuccess = MESSAGE_BANNER_UPDATE_SUCCESS;

                        model.ImageString = imageName;

                        return View(model);
                    }
                }

                catch { }

            }

            model.ImageString = banner.Image;

            ViewBag.MessageError = MESSAGE_BANNER_UPDATE_FAIL;

            return View(model);
        }
    }
}
