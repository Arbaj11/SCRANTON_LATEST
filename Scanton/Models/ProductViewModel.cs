namespace Scanton.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal MSP { get; set; }
        public decimal Weight { get; set; }
        public decimal PricePerPcs { get; set; }
        public string Stain { get; set; }
        public string Flavor { get; set; }
        public string PackSize { get; set; }
        public string StoreZipCode { get; set; }
        public string StoreName { get; set; }
        public string ImagePath { get; set; }
        public string CategoryName { get; set; }
        public string ParentCategoryName { get; set; }
    }
}
