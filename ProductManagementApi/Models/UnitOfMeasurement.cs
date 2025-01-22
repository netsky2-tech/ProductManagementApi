namespace ProductManagementApi.Models
{
    public class UnitOfMeasurement
    {
        public int UnitOfMeasurementId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
