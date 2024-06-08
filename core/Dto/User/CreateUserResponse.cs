namespace core.Dto.User;

public record CreateUserResponse
{
    public required Guid UserId { get; init; }
    public required string Email { get; init; }
    public required string UserName { get; init; }
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public long AccessTokenLifetimeSeconds { get; init; }
    public long RefreshTokenLifetimeSeconds { get; init; }
}