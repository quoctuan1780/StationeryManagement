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

        Task<List<ProductDetail>> GetListProductDetailAsync();
        Task<int> DeleteProductsDetailAsync(IList<string> productsDetailId, int productId);

        Task<ProductDetail> GetProductDetailByIdAsync(int productDetailId);
        Task<List<ProductDetail>> GetColorReportAsync(List<int> listId);
        Task<IList<ProductDetail>> GetProductDetailByIdAsync(IEnumerable<int> productDetailIds);
    }
}
