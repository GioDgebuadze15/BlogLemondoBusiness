using Blog.Data.Forms;
using Blog.Services.AppServices.PostAppService;
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

    [HttpGet("{id::int}")]
    public IActionResult GetOne(int id)
        => Ok(_iPostService.GetPostById(id));

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreatePostForm createPostForm)
        => Ok(await _iPostService.CreatePost(createPostForm));

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatePostForm updatePostForm)
        => Ok(await _iPostService.UpdatePost(updatePostForm));

    [HttpDelete("{id::int}")]
    public async Task<IActionResult> Delete(int id)
        => Ok(await _iPostService.DeletePost(id));
}