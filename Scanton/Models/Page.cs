using System.ComponentModel.DataAnnotations;

namespace Scanton.Models
{
    public class Page
    {
        [Key]
        public int CustomPageld { get; set; }

        public string CustomePageTittle { get; set; }
        public string CustomePageData { get; set; }
        public string? PageUrl { get; set; }
        public string? PageCategory { get; set; }
        public bool Is_Active { get; set; }

        public DateTime Created_date { get; set; }
        public DateTime Updated_date { get; set; }
    }
}
