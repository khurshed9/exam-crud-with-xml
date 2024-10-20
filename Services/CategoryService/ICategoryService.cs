using examWithXML.Entities;
using examWithXML.Entities.Queries;

namespace examWithXML.Services.CategoryService;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int id);
    Task<bool> CreateCategoryAsync(Category category);
    Task<bool> UpdateCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(int id);

    Task<IEnumerable<GetCategoryWithProd>> GetCategoryWithProdAsync();
}