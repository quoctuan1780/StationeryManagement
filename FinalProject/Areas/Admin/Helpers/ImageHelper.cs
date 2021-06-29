using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Common.Constant;
using static Common.ValidationConstant;

namespace FinalProject.Areas.Admin.Helpers
{
    public static class ImageHelper
    {
        public static async Task<string> SaveImageAccountAsync(IFormFile formFile, int width, int height, string userName)
        {
            if (formFile is null) return null;

            var image = Image.Load(formFile.OpenReadStream());

            image.Mutate(x =>
                x.Resize(image.Width > width ? width : image.Width, image.Height > height ? height : image.Height));

            var resultHash = HashNameImage(formFile.FileName);

            string pathString = Path.Combine(IMAGE_AVATAR_LINK, userName.Replace('+', 'a'));

            Directory.CreateDirectory(pathString);

            await image.SaveAsync(pathString + SLASH + resultHash);

            return resultHash;
        }

        public static string HashNameImage(string name)
        {
            byte[] salt;
            byte[] buffer;

            using (var hashName = new Rfc2898DeriveBytes(name, 0x10, 0x3e88))
            {
                salt = hashName.Salt;
                buffer = hashName.GetBytes(0x20);
            }

            var dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer, 0, dst, 0x11, 0x20);

            string resultHash = Convert.ToBase64String(dst).ToString() + name;

            var rgx = new Regex(VALIDATION_NON_ANPHABETIC);

            resultHash = rgx.Replace(resultHash, EMPTY);


            return resultHash;
        }

        public static async Task<string> SaveImageBannerAsync(IFormFile formFile, int width, int height)
        {
            if (formFile is null) return null;

            var image = Image.Load(formFile.OpenReadStream());

            image.Mutate(x =>
                x.Resize(image.Width > width ? width : image.Width, image.Height > height ? height : image.Height));

            var resultHash = HashNameImage(formFile.FileName);

            await image.SaveAsync(IMAGE_LINK_BANNER + resultHash);

            return resultHash;
        }
    }
}
