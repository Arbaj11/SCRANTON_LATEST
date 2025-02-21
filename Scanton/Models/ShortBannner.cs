namespace Scanton.Models
{
    public class ShortBannner
    {

        public int Id { get; set; }
        public string? Short_Banner { get; set; }
        public string? Heading_Tittle { get; set; }
        public string? Sub_Tittle_Heading { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public bool Is_Active { get; set; }
    }
}
