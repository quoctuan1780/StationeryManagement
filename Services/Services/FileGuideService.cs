using Entities.Data;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class FileGuideService : IFileGuideService
    {
        private readonly ShopDbContext _context;

        public FileGuideService(ShopDbContext context)
        {
            _context = context;
        }

        #region File Helper
        private static async Task<byte[]> GetContentBytesAsync(IFormFile file)
        {
            using var ms = new MemoryStream();

            await file.CopyToAsync(ms);

            return ms.ToArray();
        }
        #endregion

        public async Task<int> AddOrUpdateFilePdfAsync(IFormFile file, string userId, string type)
        {
            if (file is null) return 0;

            var bytes = await GetContentBytesAsync(file);

            var fileGuideExists = await _context.FileGuides.Where(x => x.Type.Equals(type)).FirstOrDefaultAsync();

            if (fileGuideExists is null)
            {

                var fileGuide = new FileGuide()
                {
                    ContentType = file.ContentType,
                    Content = Convert.ToBase64String(bytes),
                    CreatedBy = userId,
                    ModifiedBy = userId,
                    Name = file.Name,
                    Type = type
                };

                await _context.FileGuides.AddAsync(fileGuide);
            }
            else
            {
                fileGuideExists.Content = Convert.ToBase64String(bytes);
                fileGuideExists.Name = file.Name;
                fileGuideExists.ModifiedBy = userId;
                fileGuideExists.ModifiedDate = DateTime.Now;

                _context.Update(fileGuideExists);
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<FileGuide> DownloadFileAsync(int id)
        {
            return await _context.FileGuides.FindAsync(id);
        }

        public async Task<FileGuide> DownloadFileAsync(string type)
        {
            return await _context.FileGuides.Where(x => x.Type == type).FirstOrDefaultAsync();
        }
    }
}
