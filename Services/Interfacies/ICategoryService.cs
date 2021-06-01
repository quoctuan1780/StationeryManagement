using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface ICategoryService
    {
        Task<Category> AddCategoryAsync(Category category);

        Task<bool> IsExistsCategoryAsync(Category category);

        Task<IList<Category>> GetAllCategoriesAsync();

        Task<int> DeleteCategoryByIdAsync(int categoryId);

        Task<int> UpdateCategoryByIdAsync(int categoryId, string categoryName);
    }
}
