namespace ProductManagementApi.DTOs.UnitOfMeasurement
{
    public class UnitOfMeasurementUpdateDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Abbreviation { get; set; }
    }
}
