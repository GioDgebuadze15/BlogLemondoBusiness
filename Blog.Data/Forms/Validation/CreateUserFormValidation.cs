using FluentValidation;

namespace Blog.Data.Forms.Validation;

public class CreateUserFormValidation:AbstractValidator<CreateUserForm>
{
    public CreateUserFormValidation()
    {
        RuleFor(user=>user.Username)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage("Username is required!");
            
        RuleFor(user => user.Password)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage("Password is required!")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]")
            .WithMessage("Password must contain at least one digit.");

        RuleFor(user => user.ConfirmPassword)
            .Equal(user => user.Password)
            .WithMessage("Password and confirm password do not match.");
    }
}