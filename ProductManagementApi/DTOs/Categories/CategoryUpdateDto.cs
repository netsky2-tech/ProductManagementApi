namespace ProductManagementApi.DTOs
{
    public class CategoryUpdateDto
    {
        public int CategoryId { get; set; }
        public required string Name { get; set; }
        
        public string Description { get; set; }
    }
}
