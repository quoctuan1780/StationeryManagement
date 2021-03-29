using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IProductDetailService
    {
        Task<int> AddProductsDetailAsync(IList<ProductDetail> productDetail);

        Task<IList<ProductDetail>> GetProductsDetailByProductIdAsync(int productId);

        Task<int> UpdateProductsDetailAsync(IList<ProductDetail> productDetails);
    }
}
