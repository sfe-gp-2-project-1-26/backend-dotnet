using CsvHelper;
using CsvHelper.Configuration;
using ElectronicsStore.API.DTOs;
using ElectronicsStore.API.Models;
using System.Globalization;
using System.Text.Json;

namespace ElectronicsStore.API.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Check if data already exists to avoid duplication
            if (context.Products.Any() || context.Categories.Any())
            {
                return;
            }

            // Referencing the exact file name provided
            var csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Electronics_Products.csv");

            if (!File.Exists(csvFilePath))
            {
                Console.WriteLine($"CSV file not found at: {csvFilePath}");
                return;
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
                BadDataFound = null
            };

            using var reader = new StreamReader(csvFilePath);
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecords<ProductCsvRecordDto>().ToList();
            var categoryCache = new Dictionary<string, Category>();

            foreach (var record in records)
            {
                if (string.IsNullOrWhiteSpace(record.category_json)) continue;

                var categoryData = JsonSerializer.Deserialize<Dictionary<string, string>>(record.category_json);
                if (categoryData == null) continue;

                Category? lastCategory = null;
                string currentPath = "";

                // Filter out main_cat and sort sub_categories logically
                var subCategories = categoryData
                    .Where(kvp => kvp.Key != "main_cat")
                    .OrderBy(kvp => kvp.Key)
                    .Select(kvp => kvp.Value)
                    .ToList();

                foreach (var catName in subCategories)
                {
                    currentPath = string.IsNullOrEmpty(currentPath) ? catName : $"{currentPath}/{catName}";

                    if (!categoryCache.TryGetValue(currentPath, out var currentCategory))
                    {
                        currentCategory = new Category
                        {
                            Name = catName,
                            ParentCategory = lastCategory
                        };

                        context.Categories.Add(currentCategory);
                        categoryCache[currentPath] = currentCategory;
                    }

                    lastCategory = currentCategory;
                }

                if (lastCategory != null)
                {
                    // Clean the price by removing the dollar sign and commas, then parsing to decimal
                    var priceString = record.discounted_price.Replace("$", "").Replace(",", "").Trim();
                    decimal.TryParse(priceString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedPrice);

                    var product = new Product
                    {
                        Name = record.product_name,
                        Price = parsedPrice,
                        Description = record.about_product,
                        ImageUrl = record.img_link,
                        Category = lastCategory
                    };

                    context.Products.Add(product);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}