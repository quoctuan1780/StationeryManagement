using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Common.ValidationConstant;

namespace FinalProject.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = ERROR_EMAIL_EMPTY)]
        [DataType(DataType.EmailAddress, ErrorMessage = ERROR_EMAIL_TYPE)]
        public string Email { get; set; }

        [Required(ErrorMessage = ERROR_PASSWORD_ENPTY)]
        public string Password { get; set; }

        [Required(ErrorMessage = ERROR_PASSWORD_ENPTY)]
        [Compare(COMPARE_PASSWORD, ErrorMessage = ERROR_COMPARE_PADDWORD)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = ERROR_FULLNAME_EMPTY)]
        [StringLength(200, MinimumLength = 3, ErrorMessage = ERROR_FULLNAME_RANGE)]
        public string FullName { get; set; }

        [Required(ErrorMessage = ERROR_GENDER_EMPTY)]
        public string Gender { get; set; }

        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }

        [Required(ErrorMessage = ERROR_ADDRESS_EMPTY)]
        public string WardCode { get; set; }

        [Required(ErrorMessage = ERROR_PHONENUMBER_EMPTY)]
        [StringLength(11, MinimumLength = 10, ErrorMessage = ERROR_PHONENUMBER_RANGE)]
        [DataType(DataType.PhoneNumber, ErrorMessage = ERROR_PHONENUMBER_FORMAT)]
        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = ERROR_ADDRESS_EMPTY)]
        public string StreetName { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}