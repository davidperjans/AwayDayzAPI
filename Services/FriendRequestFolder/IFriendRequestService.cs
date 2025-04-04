using AwayDayzAPI.Utils;
using AwayDayzAPI.Models;
using AwayDayzAPI.DTOs;

namespace AwayDayzAPI.Services.FriendRequestFolder
{
    public interface IFriendRequestService
    {
        Task<OperationResult<string>> SendFriendRequestAsync(string senderId, string receiverUsername);
        Task<OperationResult<string>> AcceptFriendRequestAsync(int requestId, string receiverId);
        Task<OperationResult<string>> DeclineFriendRequestAsync(int requestId, string receiverId);
        Task<IEnumerable<FriendRequestDto>> GetPendingRequestsAsync(string userId);
    }
}
