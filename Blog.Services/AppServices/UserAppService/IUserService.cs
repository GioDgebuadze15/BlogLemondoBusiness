using Blog.Data.Forms;
using Blog.Data.Responses;

namespace Blog.Services.AppServices.UserAppService;

public interface IUserService
{
    Task<UserResponse> CreateUser(CreateUserForm createUserForm);
    Task<UserResponse> LoginUser(LoginUserForm loginUserForm);
}