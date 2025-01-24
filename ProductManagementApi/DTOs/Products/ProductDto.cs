namespace ProductManagementApi.DTOs.Products
{
    public class ProductDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int CategoryId { get; set; }
        public required string CategoryName { get; set; }
        public required string UnitOfMeasurementName { get; set; }
        public required decimal Price { get; set; }
    }
}
