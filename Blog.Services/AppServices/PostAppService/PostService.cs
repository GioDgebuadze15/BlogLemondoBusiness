using Blog.Data.Forms;
using Blog.Data.Models;
using Blog.Database.DatabaseRepository;
using Microsoft.Extensions.Logging;

namespace Blog.Services.AppServices.PostAppService;

public class PostService : IPostService
{
    private readonly IRepository<Post> _ctx;
    private readonly ILogger<PostService> _logger;

    public PostService(IRepository<Post> ctx, ILogger<PostService> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    public IEnumerable<Post> GetAllPosts()
        => _ctx.GetAll();

    public Post GetPostById(int id)
    {
        try
        {
            return _ctx.GetById(id).FirstOrDefault()
                   ?? throw new InvalidOperationException($"Entity with id {id} was not found.");
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, e.Message);
            return new Post();
        }
    }

    public async Task<Post> CreatePost(CreatePostForm createPostForm)
    {
        var post = new Post
        {
            Title = createPostForm.Title,
            Body = createPostForm.Body,
            DateOfPublish = createPostForm.DateOfPublish
        };
        await _ctx.Add(post);
        return post;
    }

    public async Task<Post?> UpdatePost(UpdatePostForm updatePostForm)
    {
        try
        {
            var post = _ctx.GetById(updatePostForm.Id).FirstOrDefault();
            if (post is null) throw new InvalidOperationException("Cannot find post to edit.");

            post.Title = updatePostForm.Title;
            post.Body = updatePostForm.Body;
            post.DateOfPublish = updatePostForm.DateOfPublish;

            await _ctx.Update(post);
            return post;
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, e.Message);
            return null;
        }
    }

    public async Task<Post?> DeletePost(int id)
    {
        try
        {
            var post = _ctx.GetById(id).FirstOrDefault();
            if (post is null) throw new InvalidOperationException("Cannot find post to delete.");

            await _ctx.Delete(post);
            return post;
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, e.Message);
            return null;
        }
    }
}