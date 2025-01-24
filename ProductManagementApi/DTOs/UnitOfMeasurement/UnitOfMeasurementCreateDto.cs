using System.ComponentModel.DataAnnotations;

namespace ProductManagementApi.DTOs.UnitOfMeasurement
{
    public class UnitOfMeasurementCreateDto
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }
        public string? Abbreviation { get; set; }
    }
}
