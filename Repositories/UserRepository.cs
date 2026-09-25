using Dapper;
using NotesApp.API.Data;
using NotesApp.API.Models;

namespace NotesApp.API.Repositories;

public class UserRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public UserRepository(
        DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        const string sql = """
            SELECT
                Id,
                Username,
                Email,
                PasswordHash
            FROM Users
            WHERE Email = @Email;
            """;

        using var connection =
            _dbConnectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            sql,
            new { Email = email }
        );
    }

    public async Task<int> CreateAsync(
        string username,
        string email,
        string passwordHash)
    {
        const string sql = """
            INSERT INTO Users
            (
                Username,
                Email,
                PasswordHash
            )
            OUTPUT INSERTED.Id
            VALUES
            (
                @Username,
                @Email,
                @PasswordHash
            );
            """;

        using var connection =
            _dbConnectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash
            }
        );
    }
}