using Microsoft.AspNetCore.Mvc;
using P4P.Models.DTOs.Request;
using P4P.Models.DTOs.Response;
using P4P.Services.Interfaces;

namespace P4P.Controllers;

[Route("api")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;

    public UserController(IUserService userService, IAuthenticationService authenticationService)
    {
        _userService = userService;
        _authenticationService = authenticationService;
    }

    // POST: api/Login
    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
    {
        return await _authenticationService.Login(loginRequest);
    }

    // POST: api/Register
    [HttpPost]
    [RequestSizeLimit(1024 * 1024)]
    [Route("Register")]
    public async Task<ActionResult<UserResponse>> Register([FromForm] RegisterRequest registerRequest)
    {
        return await _userService.Create(registerRequest);
    }

    // POST: api/RefreshToken
    [HttpPost]
    [Route("RefreshToken")]
    public async Task<ActionResult<RefreshTokenResponse>> RefreshToken(
        [FromBody] RefreshTokenRequest refreshTokenRequest
    )
    {
        return await _authenticationService.RefreshToken(refreshTokenRequest);
    }

    // POST: api/Logout
    [HttpPost]
    [Route("Logout")]
    public async Task<ActionResult> Logout([FromBody] LogoutRequest logoutRequest)
    {
        return await _authenticationService.Logout(logoutRequest);
    }

    // GET: api/Verify/{token}
    [HttpGet]
    [Route("Verify/{token}")]
    public async Task<GenericResponse> Verify([FromRoute] string token)
    {
        return await _authenticationService.Verify(token);
    }
}
