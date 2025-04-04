using System.Text.Json.Serialization;

namespace AwayDayzAPI.Models
{
    public class Post
    {
        public int Id { get; set; } // Primärnyckel
        public string UserId { get; set; } // Användarens ID
        public int StadiumId { get; set; } // Stadion-ID

        [JsonIgnore]
        public virtual Stadium Stadium { get; set; } // Navigation property

        public int Rating { get; set; } // 1-5
        public string Comment { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime Date { get; set; }

        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
