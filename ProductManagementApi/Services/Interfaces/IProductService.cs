using ProductManagementApi.DTOs.Products;
using ProductManagementApi.DTOs;
using ProductManagementApi.Utilities;
using ProductManagementApi.Models;

namespace ProductManagementApi.Services.Interfaces
{
    public interface IProductService
    {
        Task<PaginatedResponse<ProductDto>> GetProductsAsync(string name, int? categoryId, int pageNumber, int pageSize);
        Task<ProductDto> CreateProductAsync(ProductCreateDto productDto);
        Task UpdateProductAsync(ProductUpdateDto productDto);
        Task DeleteProductAsync(int productId);

        Task<Product?> GetProductByNameAsync(string name);
    }       
}
