namespace core.Dto.User;

public record RefreshRequest
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}