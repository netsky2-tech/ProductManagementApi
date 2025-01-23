using System.ComponentModel.DataAnnotations;

namespace ProductManagementApi.DTOs
{
    public class ProductCreateDto
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }
        public required int CategoryId { get; set; }
        public required int UnitOfMeasurementId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
        public required decimal Price { get; set; }
    }
}
