namespace ProductManagementApi.DTOs.UnitOfMeasurement
{
    public class UnitOfMeasurementDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Abbreviation { get; set; }
    }
}
