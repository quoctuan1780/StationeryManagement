using Common;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
    public class InformationClientViewModel
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = ValidationConstant.ERROR_EMAIL_EMPTY)]
        [DataType(DataType.EmailAddress, ErrorMessage = ValidationConstant.ERROR_EMAIL_TYPE)]
        public string Email { get; set; }

        [Required(ErrorMessage = ValidationConstant.ERROR_FULLNAME_EMPTY)]
        [StringLength(200, MinimumLength = 3, ErrorMessage = ValidationConstant.ERROR_FULLNAME_RANGE)]
        public string FullName { get; set; }

        [Required(ErrorMessage = ValidationConstant.ERROR_GENDER_EMPTY)]
        public string Gender { get; set; }

        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }

        [Required(ErrorMessage = ValidationConstant.ERROR_ADDRESS_EMPTY)]
        public string WardCode { get; set; }

        [Required(ErrorMessage = ValidationConstant.ERROR_PHONENUMBER_EMPTY)]
        [StringLength(11, MinimumLength = 10, ErrorMessage = ValidationConstant.ERROR_PHONENUMBER_RANGE)]
        [DataType(DataType.PhoneNumber, ErrorMessage = ValidationConstant.ERROR_PHONENUMBER_FORMAT)]
        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string ImageLink { get; set; }

        public IFormFile Image { get; set; }

        [Required(ErrorMessage = ValidationConstant.ERROR_ADDRESS_EMPTY)]
        public string StreetName { get; set; }
    }
}
