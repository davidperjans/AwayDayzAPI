using AwayDayzAPI.Enums;

namespace AwayDayzAPI.DTOs
{
    public class FriendRequestDto
    {
        public int Id { get; set; }
        public string SenderUsername { get; set; }
        public DateTime SentAt { get; set; }
        public string Status { get; set; }
    }
}
