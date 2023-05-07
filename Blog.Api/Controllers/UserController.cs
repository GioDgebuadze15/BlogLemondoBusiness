using Blog.Data.Forms;
using Blog.Services.AppServices.UserAppService;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[Route("api/user")]
public class UserController : ApiController
{
    private readonly IUserService _iUserService;

    public UserController(IUserService iUserService)
    {
        _iUserService = iUserService;
    }

    [HttpPost("SignUp")]
    public async Task<IActionResult> Create([FromBody] CreateUserForm createUserForm)
    {
        var response = await _iUserService.CreateUser(createUserForm);
        return response.StatusCode switch
        {
            StatusCodes.Status400BadRequest => BadRequest(response),
            _ => Ok(response)
        };
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> Login([FromBody] LoginUserForm loginUserForm)
    {
        var response = await _iUserService.LoginUser(loginUserForm);
        return response.StatusCode switch
        {
            StatusCodes.Status400BadRequest => BadRequest(response),
            StatusCodes.Status404NotFound => NotFound(response),
            _ => Ok(response)
        };
    }
}