namespace AwayDayzAPI.Models.DTOs.Post
{
    public class PostDto
    {
        public int StadiumId { get; set; } // Stadion-ID
        public int Rating { get; set; } // 1-5
        public string Comment { get; set; } = string.Empty;
        public bool IsPrivate { get; set; } = false; // Om inlägget är privat eller offentligt
    }
}
