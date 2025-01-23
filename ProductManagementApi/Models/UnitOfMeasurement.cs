using Microsoft.EntityFrameworkCore;

namespace ProductManagementApi.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class UnitOfMeasurement
    {
        public int UnitOfMeasurementId { get; set; }
        public required string Name { get; set; }
        public string? Abbreviation { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
