using ProductManagementApi.DTOs;
using ProductManagementApi.DTOs.Categories;
using ProductManagementApi.Models;
using ProductManagementApi.Utilities;
using SysServiciosNHME.Models;

namespace ProductManagementApi.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<PaginatedResponse<CategoryDto>> GetCategoriesAsync(string name, int pageNumber, int pageSize);
        Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto categoryDto);
        Task UpdateCategoryAsync(CategoryUpdateDto categoryDto);
        Task DeleteCategoryAsync(int categoryId);
        Task<Category?> GetCategoryByNameAsync(string name);
        Task<List<ComboDTO>> GetCategoryOptions();
    }
}
