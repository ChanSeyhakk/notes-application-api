using Microsoft.AspNetCore.Identity;
using NotesApp.API.Models;
using NotesApp.API.Repositories;

namespace NotesApp.API.Services;

public class AuthService
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(UserRepository userRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email);
    }

    public string HashPassword(
        User user,
        string password)
    {
        return _passwordHasher.HashPassword(
            user,
            password
        );
    }

    public bool VerifyPassword(
        User user,
        string password)
    {
        var result =
            _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                password
            );

        return result == PasswordVerificationResult.Success;
    }

    public async Task<int> RegisterAsync(
        string username,
        string email,
        string password)
    {
        var user = new User
        {
            Username = username,
            Email = email
        };

        var passwordHash =
            HashPassword(user, password);

        return await _userRepository.CreateAsync(
            username,
            email,
            passwordHash
        );
    }
}