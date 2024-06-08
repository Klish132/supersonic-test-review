using core.Dto.User;

namespace services.abstractions.Interfaces;

public interface IUsersService
{
    Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task RevokeTokenAsync(Guid userId);
    Task<RefreshResponse> RefreshTokenAsync(RefreshRequest request);
}