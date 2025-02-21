namespace Scanton.Models
{
    public class UserRating
    {
        // Primary Key
        public int Id { get; set; }

        // User's name
        public string UserName { get; set; }

        // User's comment (could be multi-line)
        public string Comments { get; set; }

        // Rating given by the user
        public int Rating { get; set; }

        // Indicates if the rating is active
        public bool Is_Active { get; set; }

        // Date when the rating was created
        public DateTime Created_Date { get; set; }

        // Date when the rating was last updated
        public DateTime? Updated_Date { get; set; }

        // Foreign key to Products table (ProductId)
        public int ProductId { get; set; }

        // Navigation property (if you want to include the related product)
        public virtual Product Product { get; set; }
    }
}
