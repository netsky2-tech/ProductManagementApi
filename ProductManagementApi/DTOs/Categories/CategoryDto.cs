namespace ProductManagementApi.DTOs.Categories;

public class CategoryDto
{
    public int CategoryId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

}
