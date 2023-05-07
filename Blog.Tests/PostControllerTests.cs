using Blog.Api.Controllers;
using Blog.Data.Forms;
using Blog.Data.Models;
using Blog.Data.Responses;
using Blog.Services.AppServices.PostAppService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Blog.Tests;

public class PostControllerTests
{
    private readonly Mock<IPostService> _mockPostService;
    private readonly PostController _postController;

    private readonly List<Post> _allPosts = new()
    {
        new() {Id = 1, Title = "Post 1", Body = "Body 1", DateOfPublish = new DateTime(2002, 05, 05)},
        new() {Id = 2, Title = "Post 2", Body = "Body 2", DateOfPublish = new DateTime(2003, 05, 05)},
        new() {Id = 3, Title = "Post 3", Body = "Body 3", DateOfPublish = new DateTime(2004, 05, 05)},
    };

    public PostControllerTests()
    {
        _mockPostService = new Mock<IPostService>();
        _postController = new PostController(_mockPostService.Object);
    }

    [Fact]
    public void GetAll_ReturnsOkResult_WithListOfPosts()
    {
        // Arrange
        _mockPostService.Setup(service => service.GetAllPosts()).Returns(_allPosts);

        // Act
        var result = _postController.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualPosts = Assert.IsType<List<Post>>(okResult.Value);
        Assert.Equal(_allPosts, actualPosts);
    }

    [Fact]
    public void GetBlogPostsByPublishDate_ReturnsFilteredPosts_ByPublishDate()
    {
        // Arrange
        var targetDate = new DateTime(2002, 05, 05);

        var expectedPosts = _allPosts.Where(post => post.DateOfPublish.Date == targetDate.Date).ToList();

        _mockPostService.Setup(service => service.GetPostsByPublishDate(targetDate))
            .Returns(expectedPosts);

        // Act
        var result = _postController.GetBlogPostsByPublishDate(targetDate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualPosts = Assert.IsType<List<Post>>(okResult.Value);
        Assert.Equal(expectedPosts, actualPosts);
    }

    [Fact]
    public async Task AddPost_ReturnsOkResult_WithPostResponse()
    {
        // Arrange
        var createPostForm = new CreatePostForm
        {
            Title = "New Post",
            Body = "New post body",
            DateOfPublish = new DateTime(2023, 05, 05)
        };


        var addedPost = new Post
        {
            Id = 1,
            Title = createPostForm.Title,
            Body = createPostForm.Body,
            DateOfPublish = createPostForm.DateOfPublish
        };

        var expectedPostResponse = new PostResponse
            (StatusCodes.Status200OK, null, addedPost);

        _mockPostService.Setup(service => service.CreatePost(createPostForm))
            .ReturnsAsync(expectedPostResponse);

        // Act
        var result = await _postController.Add(createPostForm);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualPostResponse = Assert.IsType<PostResponse>(okResult.Value);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(expectedPostResponse, actualPostResponse);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WithPostResponse()
    {
        // Arrange
        UpdatePostForm updatePostForm = new()
        {
            Id = 1,
            Title = "Updated Title",
            Body = "Updated Body",
            DateOfPublish = new DateTime(2005, 05, 05)
        };

        PostResponse expectedPostResponse = new
            (StatusCodes.Status404NotFound, null, null);

        _mockPostService.Setup(service => service.UpdatePost(updatePostForm))
            .ReturnsAsync(expectedPostResponse);

        // Act
        var result = await _postController.Update(updatePostForm);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualPostResponse = Assert.IsType<PostResponse>(notFoundResult.Value);

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        Assert.Equal(expectedPostResponse, actualPostResponse);
    }
    
    [Fact]
    public async Task Delete_ReturnsNotFound_WithPostResponse()
    {
        // Arrange
        const int id = 1;

        PostResponse expectedPostResponse = new
            (StatusCodes.Status404NotFound, null, null);

        _mockPostService.Setup(service => service.DeletePost(id))
            .ReturnsAsync(expectedPostResponse);

        // Act
        var result = await _postController.Delete(id);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualPostResponse = Assert.IsType<PostResponse>(notFoundResult.Value);

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        Assert.Equal(expectedPostResponse, actualPostResponse);
    }
}