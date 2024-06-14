namespace core.Dto.User;

public record UserResponse
{
    public required Guid UserId { get; init; }
    public required string Email { get; init; }
    public required string UserName { get; init; }
}