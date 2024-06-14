using System.ComponentModel.DataAnnotations;

namespace core.Dto.User;

public record CreateUserRequest
{
    [EmailAddress]
    public required string Email { get; init; }
    public required string UserName { get; init; }
    public required string Password { get; init; }
}