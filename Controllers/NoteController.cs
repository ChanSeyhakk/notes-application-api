using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApp.API.DTOs;
using NotesApp.API.Models;
using NotesApp.API.Services;

namespace NotesApp.API.Controllers;

[ApiController]
[Route("api/Notes")]
[Authorize]
public class NotesController : ControllerBase
{
    private readonly NoteService _noteService;

    public NotesController(NoteService noteService)
    {
        _noteService = noteService;
    }

    // GET: api/Notes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
    {
        int userId = GetUserId();

        var notes =
            await _noteService.GetAllAsync(userId);

        return Ok(notes);
    }

    // GET: api/Notes/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Note>> GetNote(int id)
    {
        int userId = GetUserId();

        var note =
            await _noteService.GetByIdAsync(
                id,
                userId
            );

        if (note == null)
        {
            return NotFound(new
            {
                message = "Note not found."
            });
        }

        return Ok(note);
    }

    // POST: api/Notes
    [HttpPost]
    public async Task<ActionResult> CreateNote(
        CreateNoteDto dto)
    {
        int userId = GetUserId();

        var newNoteId =
            await _noteService.CreateAsync(
                userId,
                dto.Title,
                dto.Content
            );

        return CreatedAtAction(
            nameof(GetNote),
            new
            {
                id = newNoteId
            },
            new
            {
                message = "Note created successfully.",
                id = newNoteId
            }
        );
    }

    // PUT: api/Notes/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateNote(
        int id,
        UpdateNoteDto dto)
    {
        int userId = GetUserId();

        var updated =
            await _noteService.UpdateAsync(
                id,
                userId,
                dto.Title,
                dto.Content
            );

        if (!updated)
        {
            return NotFound(new
            {
                message = "Note not found."
            });
        }

        return Ok(new
        {
            message = "Note updated successfully."
        });
    }

    // DELETE: api/Notes/5
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteNote(int id)
    {
        int userId = GetUserId();

        var deleted =
            await _noteService.DeleteAsync(
                id,
                userId
            );

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Note not found."
            });
        }

        return Ok(new
        {
            message = "Note deleted successfully."
        });
    }

    // Get logged-in user's ID from JWT
    private int GetUserId()
    {
        var userIdClaim =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

        if (string.IsNullOrEmpty(userIdClaim))
        {
            throw new UnauthorizedAccessException(
                "User ID was not found in JWT."
            );
        }

        return int.Parse(userIdClaim);
    }
}