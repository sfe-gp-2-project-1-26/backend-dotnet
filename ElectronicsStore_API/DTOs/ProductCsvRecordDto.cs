namespace ElectronicsStore.API.DTOs
{
    public class ProductCsvRecordDto
    {
        // Matching exactly the CSV headers from Electronics_Products_2.csv
        public string product_name { get; set; } = string.Empty;

        // Prices include symbols (e.g., "$2.63"), so we read them as strings initially
        public string discounted_price { get; set; } = string.Empty;
        public string actual_price { get; set; } = string.Empty;

        public string about_product { get; set; } = string.Empty;
        public string img_link { get; set; } = string.Empty;
        public string category_json { get; set; } = string.Empty;
    }
}