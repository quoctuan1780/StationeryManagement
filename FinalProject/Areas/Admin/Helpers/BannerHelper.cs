using Entities.Models;
using FinalProject.Areas.Admin.ViewModels;

namespace FinalProject.Areas.Admin.Helpers
{
    public static class BannerHelper
    {
        public static BannerViewModel ConvertBannerToBannerViewModel(Banner banner)
        {
            var model = new BannerViewModel
            {
                BannerId = banner.BannerId,
                ImageString = banner.Image,
                StartDate = banner.StartDate,
                EndDate = banner.EndDate,
                Url = banner.Url
            };

            return model;
        }
    }
}
