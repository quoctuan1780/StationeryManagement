using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IProductService
    {
        Task<IList<Product>> GetAllProductsAsync();
        Task<Product> AddProductAsync(Product product);
        Task<ProductDetail> GetProductDetailAsync(int id);
        Task<Product> UpdateProductAsync(Product product);
        Task<List<int>> ListBestSellerProduct(DateTime fromDate, DateTime toDate, int quantity);
        Task<bool> IsExistsProduct(Product product);
        Task<List<int>> GetProductDetailByProDuctIdAsync(int id);
        Task<Product> GetProductByIdAsync(int id);

        Task<IList<ProductDetail>> GetProductWithDetailsAsync();

        Task<IList<string>> GetColorByIdAsync(int productId);

        Task<IList<ProductDetail>> GetProductDetailsRunOutOfStockAsync();

        Task<string> BestSellerInMonthAsync(DateTime fromDate, DateTime toDdate, int quantity);

        Task<int> DeleteProductByIdAsync(int productId);

        Task<IList<ProductDetail>> GetTop10ProductHotAsync();

        string GetProductSkip(int skip);
    }
}
