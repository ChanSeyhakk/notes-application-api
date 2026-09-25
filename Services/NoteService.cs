using NotesApp.API.Models;
using NotesApp.API.Repositories;

namespace NotesApp.API.Services;

public class NoteService
{
    private readonly NoteRepository _noteRepository;

    public NoteService(NoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    // Get all notes
    public async Task<IEnumerable<Note>> GetAllAsync(int userId)
    {
        return await _noteRepository.GetAllAsync(userId);
    }

    // Get one note
    public async Task<Note?> GetByIdAsync(int id, int userId)
    {
        return await _noteRepository.GetByIdAsync(id, userId);
    }

    // Create note
    public async Task<int> CreateAsync(
        int userId,
        string title,
        string? content)
    {
        return await _noteRepository.CreateAsync(
            userId,
            title,
            content
        );
    }

    // Update note
    public async Task<bool> UpdateAsync(
        int id,
        int userId,
        string title,
        string? content)
    {
        return await _noteRepository.UpdateAsync(
            id,
            userId,
            title,
            content
        );
    }

    // Delete note
    public async Task<bool> DeleteAsync(int id, int userId)
    {
        return await _noteRepository.DeleteAsync(id, userId);
    }
}