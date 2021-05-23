using Common;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FinalProject.Heplers
{
    public class ImageHelper
    {
        public static void RemoveFile(IList<string> images)
        {
            if (images.FirstOrDefault() is null) return;

            var imagesRemove = images.FirstOrDefault().Split(Constant.COMMA);

            foreach (var item in imagesRemove)
            {
                File.Delete(Constant.IMAGE_LINK + item);
            }
        }

        public static async Task<string> SaveImageAsync(IFormFile formFile, int width, int height, string userName)
        {
            if (formFile is null) return null;

            var image = Image.Load(formFile.OpenReadStream());

            image.Mutate(x =>
                x.Resize(image.Width > width ? width : image.Width, image.Height > height ? height : image.Height));

            var resultHash = HashNameImage(formFile.FileName);

            string pathString = Path.Combine(Constant.IMAGE_AVATAR_LINK, userName);

            Directory.CreateDirectory(pathString);

            await image.SaveAsync(pathString + Constant.SLASH + resultHash);

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

            var rgx = new Regex(ValidationConstant.VALIDATION_NON_ANPHABETIC);

            resultHash = rgx.Replace(resultHash, Constant.EMPTY);


            return resultHash;
        }
    }
}
