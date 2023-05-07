namespace Blog.Data.Responses;

public record UserResponse(int StatusCode, string? Error, string? Token);