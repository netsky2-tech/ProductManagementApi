using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace ProductManagementApi.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Product
    {
        public int ProductId { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }

        [Precision(18, 2)]
        public required decimal Price { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }


        public required int CategoryId { get; set; }
        public Category Category { get; set; }


        public required int UnitOfMeasurementId { get; set; }
        public UnitOfMeasurement UnitOfMeasurement { get; set; }
    }
}
