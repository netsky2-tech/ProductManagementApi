using System.ComponentModel.DataAnnotations;

namespace ProductManagementApi.DTOs
{
    public class CategoryCreateDto
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }
        public string Description { get; set; }
    }
}
