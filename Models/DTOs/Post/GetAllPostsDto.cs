namespace AwayDayzAPI.Models.DTOs.Post
{
    public class GetAllPostsDto
    {
        public int Id { get; set; }
        public string StadiumName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public double Rating { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime Date { get; set; }
    }
}
