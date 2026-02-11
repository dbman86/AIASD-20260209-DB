using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PostHubAPI.Data;
using PostHubAPI.Dtos.Post;
using PostHubAPI.Exceptions;
using PostHubAPI.Models;
using PostHubAPI.Services.Interfaces;

namespace PostHubAPI.Services.Implementations;

public class PostService(ApplicationDbContext context, IMapper mapper) : IPostService
{
    public async Task<IEnumerable<ReadPostDto>> GetAllPostsAsync()
    {
        List<Post> posts = await context.Posts.Include(p => p.Comments).ToListAsync();
        return mapper.Map<IEnumerable<ReadPostDto>>(posts);
    }

    public async Task<ReadPostDto> GetPostByIdAsync(int id)
    {
        Post? post = await context.Posts.Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == id);
        if (post != null)
        {
            ReadPostDto dto = mapper.Map<ReadPostDto>(post);
            return dto;
        }

        throw new NotFoundException("Post not found!");
    }

    public async Task<int> CreateNewPostAsync(CreatePostDto dto)
    {
        Post post = mapper.Map<Post>(dto);
        context.Posts.Add(post);
        await context.SaveChangesAsync();
        return post.Id;
    }

    public async Task<ReadPostDto> EditPostAsync(int id, EditPostDto dto)
    {
        Post? postToEdit = await context.Posts.FirstOrDefaultAsync(p => p.Id == id);

        if (postToEdit != null)
        {
            mapper.Map(dto, postToEdit);
            await context.SaveChangesAsync();

            ReadPostDto readPost = mapper.Map<ReadPostDto>(postToEdit);
            return readPost;
        }

        throw new NotFoundException("Post not found!");
    }

    public async Task DeletePostAsync(int id)
    {
        Post? post = await context.Posts.FirstOrDefaultAsync(p => p.Id == id);

        if (post != null)
        {
            context.Posts.Remove(post);
            await context.SaveChangesAsync();
        }
        else
        {
            throw new NotFoundException("Post not found!");
        }
    }
}