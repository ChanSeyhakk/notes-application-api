using System.ComponentModel.DataAnnotations;

namespace NotesApp.API.DTOs;

public class CreateNoteDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Content { get; set; }
}