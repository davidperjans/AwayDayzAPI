using AwayDayzAPI.Models.DTOs.Friend;
using AwayDayzAPI.Utils;

namespace AwayDayzAPI.Services.FriendShip
{
    public interface IFriendshipService
    {
        Task<IEnumerable<FriendListDto>> GetAllFriends(string userId);
        Task<OperationResult<string>> RemoveFriend(string userId, string friendToRemove);
    }
}
