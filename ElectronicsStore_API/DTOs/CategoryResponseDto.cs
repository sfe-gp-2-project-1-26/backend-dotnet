namespace ElectronicsStore.API.DTOs
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // This will hold subcategories in a nested format
        public List<CategoryResponseDto> SubCategories { get; set; } = new List<CategoryResponseDto>();
    }
}