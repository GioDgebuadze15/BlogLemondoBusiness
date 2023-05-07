using Blog.Data.Forms;
using Blog.Data.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Blog.Services.AppServices.UserAppService;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<UserResponse> CreateUser(CreateUserForm createUserForm)
    {
        var existedUser = await _userManager.FindByNameAsync(createUserForm.Username);

        if (existedUser is not null)
            return new(StatusCodes.Status400BadRequest, "User already exists!", null);

        var user = new IdentityUser
        {
            UserName = createUserForm.Username,
            PasswordHash = createUserForm.Password
        };
        var result = await _userManager.CreateAsync(user);
        if (!result.Succeeded)
            return new(StatusCodes.Status400BadRequest, "Couldn't create user!", null);

        var token = JwtTokenHelper.GenerateJwtToken(user);
        return new(StatusCodes.Status200OK, null, token);
    }

    public async Task<UserResponse> LoginUser(LoginUserForm loginUserForm)
    {
        var existedUser = await _userManager.FindByNameAsync(loginUserForm.Username);

        if (existedUser is null)
            return new(StatusCodes.Status404NotFound, "User doesn't exists!", null);

        var result = await _signInManager.PasswordSignInAsync(existedUser, loginUserForm.Password, true, false);

        if (!result.Succeeded)
            return new(StatusCodes.Status400BadRequest, "Incorrect data!", null);

        var token = JwtTokenHelper.GenerateJwtToken(existedUser);
        return new(StatusCodes.Status200OK, null, token);
    }
}