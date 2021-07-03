using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services.Interfacies;
using System;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class GuideController : Controller
    {
        private readonly IFileGuideService _fileGuideService;
        private readonly IMemoryCache _memoryCache;

        public GuideController(IFileGuideService fileGuideService, IMemoryCache memoryCache)
        {
            _fileGuideService = fileGuideService;
            _memoryCache = memoryCache;
        }
        public IActionResult GuideBuyProduct()
        {
            return View();
        }
        public IActionResult GuidePayment()
        {
            return View();
        }

        public async Task<IActionResult> Download(string type)
        {
            bool cached = _memoryCache.TryGetValue(type, out FileGuide fileGuide);
            byte[] bytes;
            string contentType;
            if (!cached)
            {
                var result = await _fileGuideService.DownloadFileAsync(type);

                _memoryCache.Set(type, result);

                bytes = Convert.FromBase64String(result.Content);

                contentType = result.ContentType;
            }
            else
            {
                bytes = Convert.FromBase64String(fileGuide.Content);

                contentType = fileGuide.ContentType;
            }

            var file = new FileContentResult(bytes, contentType);

            return file;
        }
    }
}
