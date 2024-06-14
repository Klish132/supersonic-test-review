using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using core.Dto.User;
using core.Exceptions.User;
using core.Extensions;
using core.Jwt;
using domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using services.abstractions.Interfaces;

namespace services.Implementations;

internal class UsersService : IUsersService
{
    private readonly UserManager<User> _userManager;
    private readonly JwtOptions _jwtOptions;

    public UsersService(UserManager<User> userManager, JwtOptions jwtOptions)
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions;
    }

    public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
    {
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiry = DateTime.UtcNow.AddSeconds(_jwtOptions.RefreshTokenLifetimeSeconds);

        var user = new User
        {
            Email = request.Email,
            UserName = request.UserName,
            RefreshToken = refreshToken,
            RefreshTokenExpiryDate = refreshTokenExpiry
        };

        var res = await _userManager.CreateAsync(user, request.Password);
        if (!res.Succeeded)
            throw new UserRegistrationBadRequestHttpException(string.Join(";", 
                res.Errors.Select(e => $"{e.Code}: {e.Description}")));

        var accessToken = GenerateAccessToken(user, _jwtOptions);
        return new CreateUserResponse
        {
            UserId = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            AccessToken = accessToken,
            AccessTokenLifetimeSeconds = _jwtOptions.AccessTokenLifetimeSeconds,
            RefreshToken = refreshToken,
            RefreshTokenLifetimeSeconds = _jwtOptions.RefreshTokenLifetimeSeconds
        };
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
                   ?? throw new UserNotFoundHttpException(request.Email);
        var res = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!res)
            throw new UserUnauthorizedHttpException();
        var accessToken = GenerateAccessToken(user, _jwtOptions);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryDate = DateTime.UtcNow.AddSeconds(_jwtOptions.RefreshTokenLifetimeSeconds);
        await _userManager.UpdateAsync(user);

        return new LoginResponse
        {
            UserId = user.Id,
            Email = user.Email!,
            UserName = user.UserName!,
            AccessToken = accessToken,
            AccessTokenLifetimeSeconds = _jwtOptions.AccessTokenLifetimeSeconds,
            RefreshToken = refreshToken,
            RefreshTokenLifetimeSeconds = _jwtOptions.RefreshTokenLifetimeSeconds
        };
    }

    public async Task RevokeTokenAsync(Guid userId)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId) ??
                   throw new UserNotFoundHttpException(userId);
        user.RefreshToken = null;
        user.RefreshTokenExpiryDate = null;
        var res = await _userManager.UpdateAsync(user);
        if (!res.Succeeded)
            throw new RevokeUserRefreshTokenBadRequestHttpException(string.Join(";",
                res.Errors.Select(e => $"{e.Code}: {e.Description}")));
    }

    public async Task<RefreshResponse> RefreshTokenAsync(RefreshRequest request)
    {
        var principal = GetPrincipalFromExpiredToken(request.AccessToken, _jwtOptions);
        if (!principal.TryGetUserId(out var userId))
            throw new UserUnauthorizedHttpException();
        
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId)
                   ?? throw new UserNotFoundHttpException(userId);

        if (!IsRefreshTokenValid(request.RefreshToken, user.RefreshToken, user.RefreshTokenExpiryDate))
            throw new UserUnauthorizedHttpException();

        var accessToken = GenerateAccessToken(user, _jwtOptions);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryDate = DateTime.UtcNow.AddSeconds(_jwtOptions.RefreshTokenLifetimeSeconds);
        await _userManager.UpdateAsync(user);

        return new RefreshResponse
        {
            UserId = user.Id,
            AccessToken = accessToken,
            AccessTokenLifetimeSeconds = _jwtOptions.AccessTokenLifetimeSeconds,
            RefreshToken = refreshToken,
            RefreshTokenLifetimeSeconds = _jwtOptions.RefreshTokenLifetimeSeconds
        };
    }
    
    private static bool IsRefreshTokenValid(string refreshToken, string? currentToken, DateTime? expiryDate)
    {
        return currentToken is not null && refreshToken == currentToken
                                        && DateTime.UtcNow <= expiryDate;
    }
    
    private static ClaimsPrincipal GetPrincipalFromExpiredToken(string token, JwtOptions jwtOptions)
    {
        var secret = jwtOptions.Secret ?? throw new InvalidOperationException("Secret not configured");

        var validation = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = false
        };

        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];

        using var generator = RandomNumberGenerator.Create();

        generator.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    private static string GenerateAccessToken(User user, JwtOptions jwtOptions)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            jwtOptions.Secret ?? throw new InvalidOperationException("Secret not configured")));

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddSeconds(jwtOptions.AccessTokenLifetimeSeconds),
            claims: authClaims,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}