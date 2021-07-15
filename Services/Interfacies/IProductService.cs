using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IProductService
    {
        Task<IList<Product>> GetAllProductsAsync();
        Task<IList<Product>> GetAllProductsAsync(IList<int> productIds);
        Task<Product> AddProductAsync(Product product);
        Task<ProductDetail> GetProductDetailAsync(int id);
        Task<Product> UpdateProductAsync(Product product);
        Task<List<int>> ListBestSellerProduct(DateTime fromDate, DateTime toDate, int quantity);
        Task<bool> IsExistsProduct(Product product);
        Task<List<int>> GetProductDetailByProDuctIdAsync(int id);
        Task<Product> GetProductByIdAsync(int id);

        Task<IList<ProductDetail>> GetProductWithDetailsAsync();

        Task<IList<string>> GetColorByIdAsync(int productId);

        Task<List<ProductDetail>> GetProductDetailsRunOutOfStockAsync();

        Task<List<ProductDetail>> GetBestSellerInMonthAsync(DateTime fromDate, DateTime toDate, int quantity);
        Task<string> BestSellerInMonthAsync(DateTime fromDate, DateTime toDdate, int quantity);

        Task<int> DeleteProductByIdAsync(int productId);

        Task<IList<ProductDetail>> GetTop10ProductHotAsync();

        string GetProductSkip(int skip);

        Task<IList<Product>> GetProductsCanApplySaleAsync();
        Task<bool> CheckProductIsInAnySalesAsync(IList<int> productIds);
        Task<int> UpdatePriceSaleProductsAsync(IList<int> productIds, decimal discount);
        Task UpdateSalePriceAsync();
        Task<List<Product>> GetProductForReportExportAsync(List<int> listid);
        Task<IList<Product>> GetProductsSaleAsync();
        Task<IList<Product>> GetProductSuggestAsync(string connectionId);
    }
}
