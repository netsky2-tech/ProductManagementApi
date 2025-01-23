namespace ProductManagementApi.DTOs
{
    public class ProductUpdateDto
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public required int CategoryId { get; set; }
        public required int UnitOfMeasurementId { get; set; }
        public required decimal Price { get; set; }
    }
}
