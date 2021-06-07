﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.Shipper.ViewModels
{
    public class ChangePasswordViewModel
    {

        [DataType(DataType.Password)]
        [Required]
        [DisplayName("Mật khẩu hiện tại")]
        public string CurrentPass { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [DisplayName("Mật khẩu mới")]
        public string NewPass { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [DisplayName("Nhập lại mật khẩu")]
        [Compare("NewPass", ErrorMessage = "Mật khẩu không khớp!")]
        public string ConfirmedPass { get; set; }
    }
}
