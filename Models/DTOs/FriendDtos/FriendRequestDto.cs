using AwayDayzAPI.Enums;

namespace AwayDayzAPI.Models.DTOs.Friend
{
    public class FriendRequestDto
    {
        public int Id { get; set; }
        public string SenderUsername { get; set; }
        public DateTime SentAt { get; set; }
        public string Status { get; set; }
    }
}
