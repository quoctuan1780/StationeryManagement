using Entities.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IFileGuideService
    {
        Task<int> AddOrUpdateFilePdfAsync(IFormFile file, string userId, string type);
        Task<FileGuide> DownloadFileAsync(int id);
        Task<FileGuide> DownloadFileAsync(string type);
    }
}
