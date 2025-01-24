using Microsoft.EntityFrameworkCore;
using ProductManagementApi.DTOs;
using ProductManagementApi.DTOs.Categories;
using ProductManagementApi.Models;
using ProductManagementApi.Services.Interfaces;
using ProductManagementApi.Utilities;

namespace ProductManagementApi.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);

            if(categoryId == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedResponse<CategoryDto>> GetCategoriesAsync(string name, int pageNumber, int pageSize)
        {
            var query = _context.Categories.AsQueryable();

            if(!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }

            var totalItems = await query.CountAsync();

            var categories = await query
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync();

            return new PaginatedResponse<CategoryDto>
            {
                Items = categories,
                TotalCount = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Category?> GetCategoryByNameAsync(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<List<ComboDTO>> GetCategoryOptions()
        {
            var query = _context.Categories.AsQueryable();

            var options = await query.OrderBy(c => c.Name)
                .Select(c => new ComboDTO
                {
                    Id = c.CategoryId,
                    Name = c.Name
                }).ToListAsync();

            return options;
        }

        public async Task UpdateCategoryAsync(CategoryUpdateDto categoryDto)
        {
            var category = await _context.Categories.FindAsync(categoryDto.CategoryId);

            if (category == null)
            {
                throw new KeyNotFoundException();
            }

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;

            _context.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
