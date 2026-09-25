using Microsoft.AspNetCore.Mvc;
using NotesApp.API.Auth;
using NotesApp.API.DTOs;
using NotesApp.API.Services;

namespace NotesApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(
        AuthService authService,
        JwtTokenService jwtTokenService)
    {
        _authService = authService;
        _jwtTokenService = jwtTokenService;
    }

    // ========================================
    // REGISTER
    // ========================================

    [HttpPost("register")]
    public async Task<ActionResult> Register(
        RegisterDto dto)
    {
        var existingUser =
            await _authService.GetUserByEmailAsync(
                dto.Email
            );

        if (existingUser != null)
        {
            return Conflict(new
            {
                message = "Email is already registered."
            });
        }

        var userId =
            await _authService.RegisterAsync(
                dto.Username,
                dto.Email,
                dto.Password
            );

        return Ok(new
        {
            message = "User registered successfully.",
            userId
        });
    }


    // ========================================
    // LOGIN
    // ========================================

    [HttpPost("login")]
    public async Task<ActionResult> Login(
        LoginDto dto)
    {
        // 1. Find user by email
        var user =
            await _authService.GetUserByEmailAsync(
                dto.Email
            );

        // 2. User doesn't exist
        if (user == null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        // 3. Verify password
        var validPassword =
            _authService.VerifyPassword(
                user,
                dto.Password
            );

        // 4. Password is incorrect
        if (!validPassword)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        // 5. Generate JWT token
        var token =
            _jwtTokenService.GenerateToken(user);

        // 6. Return token
        return Ok(new
        {
            message = "Login successful.",

            token,

            user = new
            {
                id = user.Id,
                username = user.Username,
                email = user.Email
            }
        });
    }
}