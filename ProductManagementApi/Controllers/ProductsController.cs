using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Models;

namespace ProductManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // Get Products list with filters
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] string name, [FromQuery] int? categoryId)
        {
            try
            {
                var query = _context.Products.Include(p => p.Category)
                .Include(p => p.UnitOfMeasurement)
                .AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(p => p.Name.Contains(name));
                }

                if (categoryId.HasValue)
                {
                    query = query.Where(p => p.CategoryId == categoryId.Value);
                }

                var products = await query.ToListAsync();

                if (products == null || products.Count == 0)
                {
                    return NotFound(new ApiResponse<object>(false, "No se encontraron productos."));
                }

                return Ok(new ApiResponse<IEnumerable<Product>>(true, "Productos obtenidos correctamente", products));
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Ocurrió un error inesperado", null, new List<string> { ex.Message}));
            }
        }

        // Create new product
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProducts), new { productId = product.ProductId }, 
                    new ApiResponse<Product>(true, "Producto creado exitosamente", product));
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
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] Product product)
        {
            if(productId != product.ProductId)
            {
                return BadRequest(new ApiResponse<object>(false, "El ID del producto no coincide con ninguno de los guardados"));
            }

            try
            {
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<object>(true, "Producto actualizado correctamente"));

            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Ocurrió un error al actualizar el producto", null, new List<string> { ex.Message }));
            }

        }

        // Delete a product
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if(product == null)
            {
                return NotFound(new ApiResponse<object>(false, "Producto no encontrado"));
            }

            try
            {

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<object>(true, "Producto eliminado correctamente."));

            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>(false, "Ocurrió un error al eliminar el producto", null, new List<string> { ex.Message }));
            }
        }
    }
}
