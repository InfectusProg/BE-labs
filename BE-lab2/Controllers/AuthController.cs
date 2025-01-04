namespace BE_lab2.Controllers;
using BE_lab2.Contracts;
using BE_lab2.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly JWTService _jwtService;

    public AuthController(JWTService jwtService)
    {
        _jwtService = jwtService;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var result = await _jwtService.Authenticate(request);
        if (result is null)
        {
            return Unauthorized();
        }

        return result;
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<ActionResult<string>> Register(RegisterRequest request)
    {
        var result = await _jwtService.AddUserWithCurrencyAsync(request);
        if (result == null)
        {
            return Ok("User register");

        }
        return Unauthorized();
    }
}
