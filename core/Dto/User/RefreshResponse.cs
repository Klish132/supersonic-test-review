namespace core.Dto.User;

public record RefreshResponse
{
    public required Guid UserId { get; init; }
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public required long AccessTokenLifetimeSeconds { get; init; }
    public required long RefreshTokenLifetimeSeconds { get; init; }
}