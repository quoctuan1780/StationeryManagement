using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Areas.Admin.ViewModels
{
    public class UploadViewModel
    {
        [Required(ErrorMessage = "Tệp không được để trống")]
        public IFormFile File { get; set; }
    }
}
