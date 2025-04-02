using Microsoft.AspNetCore.Identity;

namespace AwayDayzAPI.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? ProfilePictureUrl { get; set; }

        public ICollection<FriendRequest> SentFriendRequests { get; set; }
        public ICollection<FriendRequest> ReceivedFriendRequests { get; set; }

        public ICollection<Friendship> FriendshipsAsUser1 { get; set; }
        public ICollection<Friendship> FriendshipsAsUser2 { get; set; }
    }
}
