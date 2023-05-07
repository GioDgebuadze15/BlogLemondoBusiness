namespace Blog.Data.Responses;

public record PostResponse(int StatusCode, string? Error, object? Date);