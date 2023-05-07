using Blog.Data.Forms;
using Blog.Services.AppServices.PostAppService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[Route("api/post")]
public class PostController : ApiController
{
    private readonly IPostService _iPostService;

    public PostController(IPostService iPostService)
    {
        _iPostService = iPostService;
    }

    [HttpGet]
    public IActionResult GetAll()
        => Ok(_iPostService.GetAllPosts());

    [HttpGet("{dateOfPublish}")]
    public IActionResult GetBlogPostsByPublishDate(DateTime dateOfPublish)
        => Ok(_iPostService.GetPostsByPublishDate(dateOfPublish));

    [HttpGet("search-posts")]
    public IActionResult GetBlogPostsByPage([FromQuery] int page, [FromQuery] int pageSize)
        => Ok(_iPostService.GetPostsByPage(page, pageSize));

    [HttpGet("{id::int}")]
    public IActionResult GetOne(int id)
    {
        var response = _iPostService.GetPostById(id);
        return response.StatusCode switch
        {
            StatusCodes.Status404NotFound => NotFound(response),
            _ => Ok(response)
        };
    }


    [HttpPost]
    // [Authorize]
    public async Task<IActionResult> Add([FromBody] CreatePostForm createPostForm)
        => Ok(await _iPostService.CreatePost(createPostForm));

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UpdatePostForm updatePostForm)
    {
        var response = await _iPostService.UpdatePost(updatePostForm);
        return response.StatusCode switch
        {
            StatusCodes.Status404NotFound => NotFound(response),
            _ => Ok(response)
        };
    }

    [HttpDelete("{id::int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _iPostService.DeletePost(id);
        return response.StatusCode switch
        {
            StatusCodes.Status404NotFound => NotFound(response),
            _ => Ok(response)
        };
    }
}