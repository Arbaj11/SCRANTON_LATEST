namespace Scanton.Models
{
    public class VideoGallery
    {
        public int Id { get; set; }         // Primary Key
        public string Title { get; set; }   // Title of the video
        public string SubTitle { get; set; } // Sub-title of the video
        public string? VideoPath { get; set; } // Path to the video file
        public bool Is_Active { get; set; }  // Status of the video
        public DateTime Created_Date { get; set; } // Date of creation
        public DateTime Updated_Date { get; set; } // Date of last update
    }
}
