using Dapper;
using NotesApp.API.Data;
using NotesApp.API.Models;

namespace NotesApp.API.Repositories;

public class NoteRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public NoteRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    // Get all notes for a user
    public async Task<IEnumerable<Note>> GetAllAsync(int userId)
    {
        const string sql = """
            SELECT
                Id,
                UserId,
                Title,
                Content,
                CreatedAt,
                UpdatedAt
            FROM Notes
            WHERE UserId = @UserId
            ORDER BY CreatedAt DESC;
            """;

        using var connection = _dbConnectionFactory.CreateConnection();

        return await connection.QueryAsync<Note>(
            sql,
            new { UserId = userId }
        );
    }

    // Get one note by ID
    public async Task<Note?> GetByIdAsync(int id, int userId)
    {
        const string sql = """
            SELECT
                Id,
                UserId,
                Title,
                Content,
                CreatedAt,
                UpdatedAt
            FROM Notes
            WHERE Id = @Id
              AND UserId = @UserId;
            """;

        using var connection = _dbConnectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Note>(
            sql,
            new
            {
                Id = id,
                UserId = userId
            }
        );
    }
    public async Task<int> CreateAsync(
    int userId,
    string title,
    string? content)
    {
        const string sql = """
        INSERT INTO Notes
        (
            UserId,
            Title,
            Content
        )
        OUTPUT INSERTED.Id
        VALUES
        (
            @UserId,
            @Title,
            @Content
        );
        """;

        using var connection = _dbConnectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                UserId = userId,
                Title = title,
                Content = content
            }
        );
    }

    public async Task<bool> UpdateAsync(
    int id,
    int userId,
    string title,
    string? content)
    {
        const string sql = """
        UPDATE Notes
        SET
            Title = @Title,
            Content = @Content,
            UpdatedAt = GETDATE()
        WHERE Id = @Id
          AND UserId = @UserId;
        """;

        using var connection = _dbConnectionFactory.CreateConnection();

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                UserId = userId,
                Title = title,
                Content = content
            }
        );

        return rowsAffected > 0;
    }

    // Delete note
    public async Task<bool> DeleteAsync(int id, int userId)
    {
        const string sql = """
        DELETE FROM Notes
        WHERE Id = @Id
          AND UserId = @UserId;
        """;

        using var connection = _dbConnectionFactory.CreateConnection();

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                UserId = userId
            }
        );

        return rowsAffected > 0;
    }
}