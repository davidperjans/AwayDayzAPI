namespace AwayDayzAPI.Models.DTOs.Post
{
    public class UpdatePostDto
    {
        public string? Comment { get; set; }
        public int? Rating { get; set; }
        public bool? IsPrivate { get; set; }
    }
}
