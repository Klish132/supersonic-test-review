using core.Dto.Note;
using core.Exceptions.Folder;
using core.Exceptions.Note;
using domain.Entities;
using domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using services.abstractions.Interfaces;
using services.Extensions;

namespace services.Implementations;

internal class NotesService : INotesService
{
    private readonly IRepositoryManager _manager;

    public NotesService(IRepositoryManager manager)
    {
        _manager = manager;
    }
    
    public async Task<CreateNoteResponse> CreateNoteAsync(Guid ownerId, CreateNoteRequest request)
    {
        var old = await _manager.Notes.Query()
            .SingleOrDefaultAsync(n => n.OwnerId == ownerId && n.Title == request.Title);
        if (old != null)
            throw new NoteTitleConflictHttpException(request.Title);
        
        if (request.FolderId != null)
            _ = await _manager.Folders.GetByIdAsync(request.FolderId.Value) ??
                throw new FolderNotFoundHttpException(request.FolderId.Value);

        var now = DateTime.UtcNow;
        var note = new Note
        {
            Title = request.Title,
            Content = request.Content,
            Significance = request.Significance,
            IsFavorite = request.IsFavorite,
            DateCreated = now,
            DateModified = now,
            FolderId = request.FolderId,
            OwnerId = ownerId
        };
        await _manager.Notes.CreateAsync(note);
        await _manager.UnitOfWork.SaveChangesAsync();
        return note.ToCreateResponse();
    }

    public async Task<UpdateNoteResponse> UpdateNoteAsync(Guid ownerId, Guid noteId, UpdateNoteRequest request)
    {
        var note = await _manager.Notes.GetByIdAsync(noteId) 
                   ?? throw new NoteNotFoundHttpException(noteId);
        if (note.OwnerId != ownerId)
            throw new NoteActionForbiddenHttpException(ownerId, noteId);

        if (request.FolderId != null)
            _ = await _manager.Folders.GetByIdAsync(request.FolderId.Value) ??
                throw new FolderNotFoundHttpException(request.FolderId.Value);

        note.Title = request.Title;
        note.Content = request.Content;
        note.Significance = request.Significance;
        note.IsFavorite = request.IsFavorite;
        note.FolderId = request.FolderId;
        note.DateModified = DateTime.UtcNow;
        
        await _manager.Notes.UpdateAsync(note);
        await _manager.UnitOfWork.SaveChangesAsync();
        return note.ToUpdateResponse();
    }

    public async Task<IEnumerable<NoteResponse>> GetUserNotesAsync(Guid ownerId, GetNotesRequest request)
    {
        var searchText = (request.SearchText ?? string.Empty).ToUpper();
        return await _manager.Notes.Query()
            .Where(n => n.OwnerId == ownerId &&
                        (n.Title.ToUpper().Contains(searchText) ||
                         n.Content.ToUpper().Contains(searchText)))
            .Select(n => n.ToNoteResponse())
            .ToListAsync();
    }

    public async Task DeleteNoteAsync(Guid ownerId, Guid noteId)
    {
        var note = await _manager.Notes.GetByIdAsync(noteId) 
                   ?? throw new NoteNotFoundHttpException(noteId);
        if (note.OwnerId != ownerId)
            throw new NoteActionForbiddenHttpException(ownerId, noteId);

        await _manager.Notes.DeleteAsync(note);
        await _manager.UnitOfWork.SaveChangesAsync();
    }

    public async Task<NoteResponse> GetNoteAsync(Guid ownerId, Guid noteId)
    {
        var note = await _manager.Notes.GetByIdAsync(noteId) 
                   ?? throw new NoteNotFoundHttpException(noteId);
        if (note.OwnerId != ownerId)
            throw new NoteActionForbiddenHttpException(ownerId, noteId);
        return note.ToNoteResponse();
    }
}