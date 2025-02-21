using System.ComponentModel.DataAnnotations;

namespace Scanton.Models
{
    public class SocialMedia
    {
        [Key]
        public int AccountId { get; set; }
        public string AccountURLLink { get; set; }
        public string AccountName { get; set; }
        public string AccountIcon { get; set; }

        public bool Is_Active { get; set; }

        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
    }
}
