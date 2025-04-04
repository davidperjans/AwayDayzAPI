using AutoMapper;
using AwayDayzAPI.Database;
using AwayDayzAPI.DTOs;
using AwayDayzAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace AwayDayzAPI.Services.FriendShip
{
    public class FriendshipService : IFriendshipService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FriendshipService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FriendListDto>> GetAllFriends(string userId)
        {
            var friendships = await _context.Friendships
                .Include(f => f.User1)
                .Include(f => f.User2)
                .Where(f => f.User1Id == userId || f.User2Id == userId)
                .ToListAsync();

            var friends = _mapper.Map<List<FriendListDto>>(friendships, opt => opt.Items["CurrentUserId"] = userId);

            return friends;
        }

        public async Task<OperationResult<string>> RemoveFriend(string userId, string friendToRemove)
        {
            var friendToRemoveId = await _context.Users.FirstOrDefaultAsync(user => user.UserName == friendToRemove);

            var friendship = _context.Friendships
                .FirstOrDefault(f => f.User1Id == userId && f.User2Id == friendToRemoveId!.Id || f.User1Id == friendToRemoveId!.Id && f.User2Id == userId);

            if (friendship == null)
                return OperationResult<string>.Failure("Friendship not found");

            // Need to find the frindrequest that was created so we can delete it so they can be friends again
            var friendRequest = await _context.FriendRequests
                .FirstOrDefaultAsync(fr => (fr.SenderId == userId && fr.ReceiverId == friendToRemoveId!.Id) ||
                                            (fr.SenderId == friendToRemoveId!.Id && fr.ReceiverId == userId));

            if (friendRequest == null)
                return OperationResult<string>.Failure("Friend request not found");

            // Remove the friend request
            _context.FriendRequests.Remove(friendRequest);

            // Remove the friendship and save
            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();

            return OperationResult<string>.Success("Friend removed successfully");
        }
    }
}
