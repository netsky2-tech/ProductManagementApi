using Microsoft.EntityFrameworkCore;
using ProductManagementApi.DTOs;
using ProductManagementApi.DTOs.Products;
using ProductManagementApi.Models;
using ProductManagementApi.Services.Interfaces;
using ProductManagementApi.Utilities;

namespace ProductManagementApi.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ProductDto> CreateProductAsync(ProductCreateDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                CategoryId = productDto.CategoryId,
                UnitOfMeasurementId = productDto.UnitOfMeasurementId,
                Price = productDto.Price
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                CategoryId = product.CategoryId,
                CategoryName = (await _context.Categories.FindAsync(product.CategoryId))?.Name,
                UnitOfMeasurementName = (await _context.UnitsOfMeasurement.FindAsync(product.UnitOfMeasurementId))?.Name,
                Price = product.Price
            };
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if(product == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedResponse<ProductDto>> GetProductsAsync(string? name, int? categoryId, int pageNumber, int pageSize)
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

            var totalItems = await query.CountAsync();

            var products = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                UnitOfMeasurementName = p.UnitOfMeasurement.Name,
                Price = p.Price
            }).ToListAsync();

            return new PaginatedResponse<ProductDto>
            {
                Items = products,
                TotalCount = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

        }

        public async Task UpdateProductAsync(ProductUpdateDto productDto)
        {
            var product = await _context.Products.FindAsync(productDto.ProductId);

            if(product == null)
            {
                throw new KeyNotFoundException();
            }

            product.Name = productDto.Name;
            product.CategoryId = productDto.CategoryId;
            product.UnitOfMeasurementId = productDto.UnitOfMeasurementId;
            product.Price = productDto.Price;

            _context.Update(product);
            await _context.SaveChangesAsync();

        }

        public async Task<Product?> GetProductByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
        }
    }
}
