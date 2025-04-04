using AutoMapper;
using AwayDayzAPI.Database;
using AwayDayzAPI.Models;
using AwayDayzAPI.Models.DTOs.Post;
using AwayDayzAPI.Utils;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace AwayDayzAPI.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PostService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OperationResult<string>> CreateAPostAsync(PostDto postDto, string userId)
        {
            var stadium = await _context.Stadiums.FindAsync(postDto.StadiumId);

            if (stadium == null)
                return OperationResult<string>.Failure("Stadium not found.");

            // Check if the user already have made an post on this stadium
            var existingPost = await _context.Posts
                .FirstOrDefaultAsync(p => p.UserId == userId && p.StadiumId == postDto.StadiumId);

            if (existingPost != null)
                return OperationResult<string>.Failure("You have already made a post on this stadium.");

            var newPost = new Post
            {
                UserId = userId,
                StadiumId = postDto.StadiumId,
                Rating = postDto.Rating,
                Comment = postDto.Comment,
                IsPrivate = postDto.IsPrivate,
                Date = DateTime.UtcNow,
            };

            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();

            return OperationResult<string>.Success("Created new post successfully");
        }

        public async Task<OperationResult<string>> DeletePostAsync(int postId, string userId)
        {
            var postToDelete = await _context.Posts
                .FirstOrDefaultAsync(p => p.Id == postId && p.UserId == userId);

            if (postToDelete == null)
                return OperationResult<string>.Failure("Post not found or you are not authorized to delete this post.");

            _context.Posts.Remove(postToDelete);
            await _context.SaveChangesAsync();

            return OperationResult<string>.Success("Deleted the post successfully");
        }

        public async Task<IEnumerable<GetAllPostsDto>> GetAllPostsAsync(string userId)
        {
            var allPosts = _context.Posts
                .Include(p => p.Stadium)
                .Where(p => p.UserId == userId).ToList();

            var postDtos = _mapper.Map<List<GetAllPostsDto>>(allPosts);

            return postDtos;
        }

        public async Task<OperationResult<string>> PatchPostAsync(int postId, string userId, UpdatePostDto updatePostDto)
        {
            var postToPatch = await _context.Posts.FindAsync(postId);

            if (postToPatch == null)
                return OperationResult<string>.Failure("Post not found.");

            // Check if the user is the owner of the post
            if (postToPatch.UserId != userId)
                return OperationResult<string>.Failure("You are not authorized to update this post.");

            // Update the post
            if (updatePostDto.Rating != null)
                postToPatch.Rating = updatePostDto.Rating.Value;

            if (updatePostDto.Comment != null)
                postToPatch.Comment = updatePostDto.Comment;

            if (updatePostDto.IsPrivate != null)
                postToPatch.IsPrivate = updatePostDto.IsPrivate.Value;

            await _context.SaveChangesAsync();

            return OperationResult<string>.Success("Post updated successfully");
        }
    }
}
