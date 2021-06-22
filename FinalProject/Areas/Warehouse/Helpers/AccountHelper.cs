using Entities.Models;
using FinalProject.Areas.Warehouse.ViewModels;

namespace FinalProject.Areas.Warehouse.Helpers
{
    public class AccountHelper
    {
        public static InformationViewModel ConvertFromUserToInformationClientViewModel(User user)
        {
            var model = new InformationViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                ImageLink = user.Image,
                FullName = user.FullName,
                Gender = user.Gender,
                ProvinceId = user.Ward.District.Province.ProvinceId,
                DistrictId = user.Ward.District.DistrictId,
                WardCode = user.WardCode,
                PhoneNumber = user.PhoneNumber,
                StreetName = user.StreetName
            };

            return model;
        }
    }
}
