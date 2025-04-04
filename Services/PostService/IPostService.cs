using AwayDayzAPI.Models;
using AwayDayzAPI.Models.DTOs.Post;
using AwayDayzAPI.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AwayDayzAPI.Services.PostService
{
    public interface IPostService
    {
        Task<OperationResult<string>> CreateAPostAsync(PostDto postDto, string userId);
        Task<OperationResult<string>> DeletePostAsync(int postId, string userId);
        Task<IEnumerable<GetAllPostsDto>> GetAllPostsAsync(string userId);
        Task<OperationResult<string>> PatchPostAsync(int postId, string userId, UpdatePostDto updatePostDto);
    }
}
