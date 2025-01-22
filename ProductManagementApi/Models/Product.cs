namespace ProductManagementApi.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; }


        public int UnitOfMeasurementId { get; set; }
        public UnitOfMeasurement UnitOfMeasurement { get; set; }
    }
}
