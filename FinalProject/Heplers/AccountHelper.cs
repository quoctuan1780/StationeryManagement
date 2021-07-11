using Entities.Models;
using FinalProject.Areas.Admin.ViewModels;
using FinalProject.ViewModels;

namespace FinalProject.Heplers
{
    public class AccountHelper
    {
        public static InformationClientViewModel ConvertFromUserToInformationClientViewModel(User user)
        {
            var model = new InformationClientViewModel
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

        public static InformationViewModel ConvertFromUserToInformationViewModel(User user)
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
