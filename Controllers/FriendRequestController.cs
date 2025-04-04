using System.Security.Claims;
using AwayDayzAPI.Services.FriendRequestFolder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AwayDayzAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendRequestController : ControllerBase
    {
        private readonly IFriendRequestService _friendRequestService;

        public FriendRequestController(IFriendRequestService friendRequestService)
        {
            _friendRequestService = friendRequestService;
        }

        [HttpPost("send-friendrequest")]
        public async Task<IActionResult> SendFriendRequest([FromQuery] string receiverUsername)
        {
            // Get who sends the friend request
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(senderId))
                return BadRequest("Sender ID not found");

            var result = await _friendRequestService.SendFriendRequestAsync(senderId, receiverUsername);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            
            return Ok(result.Data);
        }

        [HttpPost("accept-friendrequest")]
        public async Task<IActionResult> AcceptFriendRequest([FromQuery] int requestId)
        {
            // Get who accepts the friend request
            var receiverId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(receiverId))
                return BadRequest("Receiver ID not found");

            var result = await _friendRequestService.AcceptFriendRequestAsync(requestId, receiverId);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpPost("decline-friendrequest")]
        public async Task<IActionResult> DeclineFriendRequest([FromQuery] int requestId)
        {
            // Get who declines the friend request
            var receiverId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(receiverId))
                return BadRequest("Receiver ID not found");

            var result = await _friendRequestService.DeclineFriendRequestAsync(requestId, receiverId);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("pending-requests")]
        public async Task<IActionResult> GetPendingRequest()
        {
            // Get who gets the pending requests
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID not found");

            var result = await _friendRequestService.GetPendingRequestsAsync(userId);
            if (result == null)
                return NotFound("No pending requests found");

            return Ok(result);
        }
    }
}
