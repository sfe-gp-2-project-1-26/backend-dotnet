namespace ElectronicsStore.API.Models
{
    public class Category
    {
        // Primary key for the category
        public int Id { get; set; }

        // The name of the category (e.g., Laptops, Mobile Phones)
        public string Name { get; set; } = string.Empty;

        // Self-referencing foreign key for subcategories (nullable because root categories have no parent)
        public int? ParentCategoryId { get; set; }

        // Navigation property to the parent category
        public Category? ParentCategory { get; set; }

        // Navigation property to child subcategories
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();

        // Navigation property to products under this specific category
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}