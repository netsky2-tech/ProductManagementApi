using Microsoft.EntityFrameworkCore;
using ProductManagementApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Models;
using ProductManagementApi.Services.Interfaces;
using ProductManagementApi.Utilities;
using ProductManagementApi.DTOs.Categories;

namespace ProductManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly AppDbContext _context;

        public CategoriesController(ICategoryService categoryService, AppDbContext context)
        {
            _categoryService = categoryService;
            _context = context;
        }   

        //Get categories with filters
        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] string? name, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest(new ApiResponse<object>(false, "Parámetros de paginación inválidos."));
            }

            try
            {
                var result = await _categoryService.GetCategoriesAsync(name, pageNumber, pageSize);
                return Ok(new ApiResponse<PaginatedResponse<CategoryDto>>(true, "Categorias obtenidas exitosamente", result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Ha ocurrido un error inesperado", null, new List<string> { ex.Message }));
            }
        }

        // Create new category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest(new ApiResponse<object>(false, "Los datos de la categoria no pueden estar vacios."));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>(false, "Datos inválidos.", null, ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
            }

            try
            {
                var existingCategory = await _categoryService.GetCategoryByNameAsync(categoryDto.Name);
                if(existingCategory != null)
                {
                    return BadRequest(new ApiResponse<object>(false, "Ya existe una categoria con el mismo nombre."));
                }

                using var transaction = await _context.Database.BeginTransactionAsync();

                var result = await _categoryService.CreateCategoryAsync(categoryDto);

                await transaction.CommitAsync();

                return Ok(new ApiResponse<CategoryDto>(true, "Categoria creada existosamente", result));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Error de base de datos.", null, new List<string> { ex.InnerException?.Message ?? ex.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Ha ocurrido un error inesperado", null, new List<string> { ex.Message }));
            }
        }

        //Update a category
        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryUpdateDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest(new ApiResponse<object>(false, "Los datos de la categoria no pueden estar vacios."));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>(false, "Datos inválidos.", null, ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
            }

            try
            {
                await _categoryService.UpdateCategoryAsync(categoryDto);
                return Ok(new ApiResponse<object>(true, "Categoria actualizada exitosamente"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<object>(false, "Categoria no encontrada"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Ha ocurrido un error inesperado", null, new List<string> { ex.Message }));
            }
        }

        //Delete a category
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(categoryId);
                return Ok(new ApiResponse<object>(true, "Categoria eliminada exitosamente"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<object>(false, "Categoria no encontrada"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Ha ocurrido un error inesperado", null, new List<string> { ex.Message }));
            }
        }

        //Get options for combo boxes
        [HttpGet("options")]
        public async Task<IActionResult> GetOptions()
        {
            try
            {
                var result = await _categoryService.GetCategoryOptions();

                return Ok(new ApiResponse<object>(true, "Categorias obtenidas correctaemnte", result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Ha ocurrido un error inesperado", null, new List<string> { ex.Message }));
            }
        }
    }
}
