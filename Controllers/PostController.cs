using System.Security.Claims;
using AwayDayzAPI.Models.DTOs.Post;
using AwayDayzAPI.Services.PostService;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AwayDayzAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }


        [HttpPost("create-a-post")]
        public async Task<IActionResult> CreatePost([FromBody] PostDto postDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized("User not found.");

            var result = await _postService.CreateAPostAsync(postDto, userId);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpDelete("delete-post/{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized("User not found.");

            var result = await _postService.DeletePostAsync(postId, userId);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("get-posts")]
        public async Task<IActionResult> GetAllPosts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized("User not found.");

            var posts = await _postService.GetAllPostsAsync(userId);
            return Ok(posts);
        }

        [HttpPatch("update-post/{postId}")]
        public async Task<IActionResult> UpdatePost(int postId, [FromBody] UpdatePostDto updatePostDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized("User not found.");

            var result = await _postService.PatchPostAsync(postId, userId, updatePostDto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }
    }
}
