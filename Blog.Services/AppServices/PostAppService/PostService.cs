using Blog.Data.Forms;
using Blog.Data.Models;
using Blog.Data.Responses;
using Blog.Data.ViewModels;
using Blog.Database.DatabaseRepository;
using Microsoft.AspNetCore.Http;
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

    public IEnumerable<Post> GetPostsByPublishDate(DateTime dateOfPublish)
        => _ctx.GetAll().Where(x => x.DateOfPublish == dateOfPublish).AsEnumerable();

    public IEnumerable<Post> GetPostsByPage(int page, int pageSize)
        => _ctx.GetAll().Skip((page - 1) * pageSize).Take(pageSize).AsEnumerable();

    public PostResponse GetPostById(int id)
    {
        try
        {
            var post = _ctx.GetById(id).FirstOrDefault()
                       ?? throw new InvalidOperationException($"Entity with id {id} was not found.");
            return new(StatusCodes.Status200OK, null, PostViewModels.Default.Compile().Invoke(post));
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, e.Message);
            return new(StatusCodes.Status404NotFound, e.Message, null);
        }
    }

    public async Task<PostResponse> CreatePost(CreatePostForm createPostForm)
    {
        var post = new Post
        {
            Title = createPostForm.Title,
            Body = createPostForm.Body,
            DateOfPublish = createPostForm.DateOfPublish
        };
        await _ctx.Add(post);
        return new(StatusCodes.Status200OK, null, PostViewModels.Default.Compile().Invoke(post));
    }

    public async Task<PostResponse> UpdatePost(UpdatePostForm updatePostForm)
    {
        try
        {
            var post = _ctx.GetById(updatePostForm.Id).FirstOrDefault();
            if (post is null) throw new InvalidOperationException("Cannot find post to edit.");

            post.Title = updatePostForm.Title;
            post.Body = updatePostForm.Body;
            post.DateOfPublish = updatePostForm.DateOfPublish;

            await _ctx.Update(post);
            return new(StatusCodes.Status200OK, null, PostViewModels.Default.Compile().Invoke(post));
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, e.Message);
            return new(StatusCodes.Status404NotFound, e.Message, null);
        }
    }

    public async Task<PostResponse> DeletePost(int id)
    {
        try
        {
            var post = _ctx.GetById(id).FirstOrDefault();
            if (post is null) throw new InvalidOperationException("Cannot find post to delete.");

            await _ctx.Delete(post);
            return new(StatusCodes.Status200OK, null, PostViewModels.Default.Compile().Invoke(post));
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, e.Message);
            return new(StatusCodes.Status404NotFound, e.Message, null);
        }
    }
}