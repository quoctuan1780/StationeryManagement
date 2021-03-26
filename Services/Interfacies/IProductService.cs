using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IProductService
    {
        Task<IList<Product>> GetAllProductsAsync();

        Task<Product> AddProductAsync(Product product);

        Task<Product> UpdateProductAsync(Product product);

        Task<bool> IsExistsProduct(Product product);

        Task<Product> GetProductByIdAsync(int id);
    }
}
