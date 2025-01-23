using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagementApi.DTOs;
using ProductManagementApi.DTOs.Products;
using ProductManagementApi.Models;
using ProductManagementApi.Services.Interfaces;
using ProductManagementApi.Utilities;
using System.Transactions;

namespace ProductManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly AppDbContext _context;
        public ProductsController(IProductService productService, AppDbContext context)
        {
            _productService = productService;
            _context = context;
        }

        // Get Products list with filters
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] string? name, [FromQuery] int? categoryId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest(new ApiResponse<object>(false, "Parámetros de paginación inválidos."));
            }

            try
            {
                var result = await _productService.GetProductsAsync(name, categoryId, pageNumber, pageSize);
                return Ok(new ApiResponse<PaginatedResponse<ProductDto>>(true, "Productos obtenidos correctamente", result));

            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Ocurrió un error inesperado", null, new List<string> { ex.Message}));
            }
        }

        // Create new product
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest(new ApiResponse<object>(false, "Los datos del producto no pueden ser nulos."));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>(false, "Datos inválidos.", null, ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
            }

            try
            {
                var existingProduct = await _productService.GetProductByNameAsync(productDto.Name);
                if(existingProduct != null)
                {
                    return Conflict(new ApiResponse<object>(false, "Ya existe un producto con el mismo nombre."));
                }

                using var transaction = await _context.Database.BeginTransactionAsync();

                var product = await _productService.CreateProductAsync(productDto);

                await transaction.CommitAsync();

                return Ok(new ApiResponse<ProductDto>(true, "Producto creado exitosamente", product));

            }
            catch(DbUpdateException ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Error de base de datos.", null, new List<string> { ex.InnerException?.Message ?? ex.Message }));
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Ocurrio un error al crear el producto", null, new List<string> { ex.Message }));
            }

        }

        // Update a product
        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] ProductUpdateDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>(false, "Datos inválidos.", null, ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
            }

            if (productId != productDto.ProductId)
            {
                return BadRequest(new ApiResponse<object>(false, "El ID del producto no coincide con el enviado."));
            }

            try
            {
                await _productService.UpdateProductAsync(productDto);

                return Ok(new ApiResponse<object>(true, "Producto actualizado correctamente"));

            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new ApiResponse<object>(false, "Producto no encontrado."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Ocurrió un error al actualizar el producto.", null, new List<string> { ex.Message }));
            }

        }

        // Delete a product
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {

                await _productService.DeleteProductAsync(productId);

                return Ok(new ApiResponse<object>(true, "Producto eliminado correctamente."));

            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<object>(false, "Producto no encontrado."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Ocurrió un error al eliminar el producto.", null, new List<string> { ex.Message }));
            }
        }
    }
}
