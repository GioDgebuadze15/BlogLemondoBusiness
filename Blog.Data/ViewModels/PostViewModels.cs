using System.Linq.Expressions;
using Blog.Data.Models;

namespace Blog.Data.ViewModels;

public static class PostViewModels
{
    public static Expression<Func<Post, object>> Default =>
        post => new
        {
            post.Id,
            post.Title,
            post.Body,
            post.DateOfPublish
        };
}