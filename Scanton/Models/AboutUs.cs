namespace Scanton.Models
{
    public class AboutUs
    {
        public int Id { get; set; }
        public string? Banner_Image { get; set; }
        public string Banner_Title { get; set; }
        public string Banner_SubTitle { get; set; }
        public string? Product_Image1 { get; set; }
        public string Product1_Title { get; set; }
        public string Product1_Description { get; set; }
        public string? Product_Image2 { get; set; }
        public string Product2_Title { get; set; }
        public string Product2_Description { get; set; }
        public string? VideoPath { get; set; }
        public string VideoTitle { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime? Updated_Date { get; set; }
        public bool Is_Active { get; set; }
    }
}
