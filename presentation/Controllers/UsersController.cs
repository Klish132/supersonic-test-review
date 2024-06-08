using core.Dto.User;
using core.Exceptions.User;
using core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using services.abstractions.Interfaces;

namespace presentation.Controllers;

[ApiController]
[Route("api/auth")]
public class UsersController : Controller
{
    private readonly IServicesManager _manager;
    
    public UsersController(IServicesManager manager)
    {
        _manager = manager;
    }
    
    [HttpPost("register")]
    [ProducesResponseType<CreateUserResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        var response = await _manager.Users.CreateUserAsync(request);
        return Ok(response);
    }

    [HttpPost("login")]
    [ProducesResponseType<LoginResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _manager.Users.LoginAsync(request);
        return Ok(response);
    }
    
    [HttpPost("refresh")]
    [ProducesResponseType<RefreshResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
    {
        var response = await _manager.Users.RefreshTokenAsync(request);
        return Ok(response);
    }
    
    [Authorize]
    [HttpPost("revoke")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RevokeToken()
    {
        if (!User.TryGetUserId(out var userId))
            throw new UserUnauthorizedHttpException();
        await _manager.Users.RevokeTokenAsync(userId);
        return Ok();
    }
}