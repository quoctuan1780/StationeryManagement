using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IProductImageService
    {
        Task<int> AddListImagesAsync(IList<ProductImage> productImages);

        Task<int> DeleteListImagesOfProductByNameAsync(IList<string> images, int id);
    }
}
