using FluentValidation;

namespace Blog.Data.Forms.Validation;

public class LoginUserFormValidation:AbstractValidator<LoginUserForm>
{
    public LoginUserFormValidation()
    {
        RuleFor(user=>user.Username)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage("Username is required!");
        
        RuleFor(user=>user.Password)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage("Password is required!");
    }
}