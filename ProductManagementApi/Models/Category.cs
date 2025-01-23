using Microsoft.EntityFrameworkCore;

namespace ProductManagementApi.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        public int CategoryId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }


        public ICollection<Product> Products { get; set; }
    }
}
