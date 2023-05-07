using FluentValidation;

namespace Blog.Data.Forms.Validation;

public class CreatePostFormValidation : AbstractValidator<CreatePostForm>
{
    public CreatePostFormValidation()
    {
        RuleFor(post => post.Title)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage("Title is required!");

        RuleFor(post => post.Body)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage("Body is required!");

        RuleFor(post => post.DateOfPublish)
            .NotEmpty()
            .WithMessage("DateOfPublish is required!")
            .Must(x => x > DateTime.MinValue && x < DateTime.MaxValue)
            .WithMessage("DateTimeProperty must be a valid date and time.");
    }
}