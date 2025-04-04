using AutoMapper;
using AutoMapper.QueryableExtensions;
using AwayDayzAPI.Database;
using AwayDayzAPI.Enums;
using AwayDayzAPI.Models;
using AwayDayzAPI.Models.DTOs.Friend;
using AwayDayzAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace AwayDayzAPI.Services.FriendRequestFolder
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FriendRequestService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OperationResult<string>> SendFriendRequestAsync(string senderId, string receiverUsername)
        {
            // Get the receiver user so we can access its ID
            var receiver = _context.Users.FirstOrDefault(u => u.UserName == receiverUsername);

            if (receiver == null)
                return OperationResult<string>.Failure("Receiver not found");

            // Check if friendrequest already exist
            var existingRequest = await _context.FriendRequests.FirstOrDefaultAsync(fr =>
                (fr.SenderId == senderId && fr.ReceiverId == receiver.Id) ||
                (fr.SenderId == receiver.Id && fr.ReceiverId == senderId));

            if (existingRequest != null)
                return OperationResult<string>.Failure("Friend request already exists");

            // Create a new friendrequest
            var friendRequest = new FriendRequest
            {
                SenderId = senderId,
                ReceiverId = receiver.Id,
                SentAt = DateTime.UtcNow,
                Status = RequestStatus.Pending
            };

            // Add the friendrequest to the database
            await _context.FriendRequests.AddAsync(friendRequest);
            await _context.SaveChangesAsync();

            return OperationResult<string>.Success("Friend request sent successfully");
        }
        public async Task<OperationResult<string>> AcceptFriendRequestAsync(int requestId, string receiverId)
        {
            // Get the friend request
            var friendRequest = await _context.FriendRequests
                .FirstOrDefaultAsync(fr => fr.Id == requestId && fr.ReceiverId == receiverId);

            if (friendRequest == null)
                return OperationResult<string>.Failure("Friend request not found or you are not the receiver");

            // Check if the request is already accepted or declined
            if (friendRequest.Status != RequestStatus.Pending)
                return OperationResult<string>.Failure("Friend request already accepted or declined");

            // Update the status of the friend request
            friendRequest.Status = RequestStatus.Accepted;
            friendRequest.AcceptedOrRejectedAt = DateTime.UtcNow;

            // Add friendship between the users
            var friendship = new Friendship
            {
                User1Id = friendRequest.SenderId,
                User2Id = friendRequest.ReceiverId,
                StartedAt = DateTime.UtcNow
            };

            _context.FriendRequests.Update(friendRequest);
            _context.Friendships.Add(friendship);
            await _context.SaveChangesAsync();

            return OperationResult<string>.Success("Friend request accepted successfully");
        }

        public async Task<OperationResult<string>> DeclineFriendRequestAsync(int requestId, string receiverId)
        {
            // Get the friend request
            var friendRequest = await _context.FriendRequests
                .FirstOrDefaultAsync(fr => fr.Id == requestId && fr.ReceiverId == receiverId);

            if (friendRequest == null)
                return OperationResult<string>.Failure("Friend request not found or you are not the receiver");

            // Check if the request is already accepted or declined
            if (friendRequest.Status != RequestStatus.Pending)
                return OperationResult<string>.Failure("Friend request already accepted or declined");

            // Update the status of the friend request
            friendRequest.Status = RequestStatus.Declined;
            friendRequest.AcceptedOrRejectedAt = DateTime.UtcNow;

            _context.FriendRequests.Update(friendRequest);
            await _context.SaveChangesAsync();

            return OperationResult<string>.Success("Friend request declined successfully");
        }

        public async Task<IEnumerable<FriendRequestDto>> GetPendingRequestsAsync(string userId)
        {
            return await _context.FriendRequests
                .Where(fr => fr.ReceiverId == userId && fr.Status == RequestStatus.Pending)
                .ProjectTo<FriendRequestDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
