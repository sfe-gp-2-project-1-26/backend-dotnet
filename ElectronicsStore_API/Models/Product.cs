namespace ElectronicsStore.API.Models
{
    public class Product
    {
        // Primary key for the product
        public int Id { get; set; }

        // Product name or title
        public string Name { get; set; } = string.Empty;

        // Detailed description of the product
        public string? Description { get; set; }

        // Product price
        public decimal Price { get; set; }

        // URL for the product image 
        public string? ImageUrl { get; set; }

        // Foreign Key linking the product to a specific category
        public int CategoryId { get; set; }

        // Navigation property to the related category
        public Category? Category { get; set; }
    }
}