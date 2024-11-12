using Microsoft.AspNetCore.Mvc;
using Service.DTOs;

[ApiController]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [Route("v1/Auth/Login")]
    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        try
        {
            var token = _authService.AuthenticateUser(email, password);
            return Ok(new { Token = token });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid credentials");
        }
    }

    [Route("v1/User/CreateUser")]
    [HttpPost]
    public IActionResult CreateUser(CreateUserDTO userDto)
    {
        try
        {
            _authService.CreateUser(userDto);
            return Ok("User created successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Auth/RequestPasswordReset")]
    [HttpPost]
    public IActionResult RequestPasswordReset(string email)
    {
        try
        {
            _authService.RequestPasswordReset(email);
            return Ok("Password reset code sent to your email.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Auth/ResetPassword")]
    [HttpPost]
    public IActionResult ResetPassword(string email, string newPassword, string confirmPassword, string code)
    {
        if (newPassword != confirmPassword)
            return BadRequest("Passwords do not match.");

        try
        {
            _authService.ResetPassword(email, newPassword, confirmPassword, code);
            return Ok("Password has been reset successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}