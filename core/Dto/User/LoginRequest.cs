using System.ComponentModel.DataAnnotations;

namespace core.Dto.User;

public record LoginRequest
{
    [EmailAddress]
    public required string Email { get; init; }
    public required string Password { get; init; }
}