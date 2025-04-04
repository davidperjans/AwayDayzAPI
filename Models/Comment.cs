namespace AwayDayzAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; } // Foreign key to Post
        public virtual Post Post { get; set; } // Navigation property

        public string UserId { get; set; } // Användaren som kommenterars ID
        public string CommentText { get; set; } // Själva kommentaren
    }
}
