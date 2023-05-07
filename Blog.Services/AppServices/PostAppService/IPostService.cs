using Blog.Data.Forms;
using Blog.Data.Models;
using Blog.Data.Responses;

namespace Blog.Services.AppServices.PostAppService;

public interface IPostService
{
    IEnumerable<Post> GetAllPosts();
    IEnumerable<Post> GetPostsByPublishDate(DateTime dateOfPublish);
    PostResponse GetPostById(int id);
    Task<PostResponse> CreatePost(CreatePostForm createPostForm);
    Task<PostResponse> UpdatePost(UpdatePostForm updatePostForm);
    Task<PostResponse> DeletePost(int id);

}