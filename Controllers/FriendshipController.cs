using System.Security.Claims;
using AwayDayzAPI.Services.FriendShip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AwayDayzAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipService _friendshipService;

        public FriendshipController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        [HttpGet("get-all-friends")]
        public async Task<IActionResult> GetAllFriends()
        {
            // Get the user ID from the token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID not found");

            var friends = await _friendshipService.GetAllFriends(userId);
            return Ok(friends);
        }

        [HttpDelete("remove-friend")]
        public async Task<IActionResult> RemoveFriend([FromQuery] string friendToRemove)
        {
            // Get the user ID from the token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID not found");

            var result = await _friendshipService.RemoveFriend(userId, friendToRemove);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }
    }
}
