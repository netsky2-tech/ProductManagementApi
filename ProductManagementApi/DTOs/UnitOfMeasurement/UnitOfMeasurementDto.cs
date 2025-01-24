namespace ProductManagementApi.DTOs.UnitOfMeasurement
{
    public class UnitOfMeasurementDto
    {
        public int UnitOfMeasurementId { get; set; }
        public required string Name { get; set; }
        public string? Abbreviation { get; set; }
    }
}
