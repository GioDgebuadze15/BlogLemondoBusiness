using Blog.Data.Forms;
using Blog.Data.Models;

namespace Blog.Services.AppServices.PostAppService;

public interface IPostService
{
    IEnumerable<Post> GetAllPosts();
    Post GetPostById(int id);
    Task<Post> CreatePost(CreatePostForm createPostForm);
    Task<Post?> UpdatePost(UpdatePostForm updatePostForm);
    Task<Post?> DeletePost(int id);

}