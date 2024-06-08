using core.Dto.Note;

namespace services.abstractions.Interfaces;

public interface INotesService
{
    public Task<CreateNoteResponse> CreateNoteAsync(Guid ownerId, CreateNoteRequest request);
    public Task<UpdateNoteResponse> UpdateNoteAsync(Guid ownerId, Guid noteId, UpdateNoteRequest request);
    public Task<IEnumerable<NoteResponse>> GetUserNotesAsync(Guid ownerId, GetNotesRequest request);
    public Task DeleteNoteAsync(Guid ownerId, Guid noteId);
    public Task<NoteResponse> GetNoteAsync(Guid ownerId, Guid noteId);
}