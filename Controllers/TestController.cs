using Microsoft.AspNetCore.Mvc;
using NotesApp.API.Data;

namespace NotesApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public TestController(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    [HttpGet("database")]
    public async Task<IActionResult> TestDatabase()
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            await connection.OpenAsync();

            return Ok(new
            {
                message = "Database connection successful!"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Database connection failed.",
                error = ex.Message
            });
        }
    }
}