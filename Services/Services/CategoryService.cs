using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ShopDbContext _context;

        public CategoryService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<Category> AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);

            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<int> DeleteCategoryByIdAsync(int categoryId)
        {
            var result = await _context.Categories.FindAsync(categoryId);

            if(!(result is null))
            {
                result.IsDeleted = true;

                _context.Categories.Update(result);

                return await _context.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<IList<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<bool> IsExistsCategoryAsync(Category category)
        {
            var result = await _context.Categories.Where(x => x.CategoryName == category.CategoryName).FirstOrDefaultAsync();

            if (result is null)
            {
                return true;
            }

            return false;
        }

        public async Task<int> UpdateCategoryByIdAsync(int categoryId, string categoryName)
        {
            var category = await _context.Categories.FindAsync(categoryId);

            if(!(category is null))
            {
                category.CategoryName = categoryName;

                bool isExists = await IsExistsCategoryAsync(category);

                if (!isExists)
                {
                    return -1;
                }

                _context.Categories.Update(category);

                return await _context.SaveChangesAsync();
            }

            return 0;
        }
    }
}
