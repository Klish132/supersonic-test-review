using core.Dto.Note;
using domain.Entities;

namespace services.Extensions;

public static class NoteExtensions
{
    public static CreateNoteResponse ToCreateResponse(this Note note)
    {
        return new CreateNoteResponse
        {
            NoteId = note.Id,
            Title = note.Title,
            Content = note.Content,
            Significance = note.Significance,
            IsFavorite = note.IsFavorite,
            DateCreated = note.DateCreated,
            DateModified = note.DateModified,
            OwnerId = note.OwnerId,
            FolderId = note.FolderId
        };
    }

    public static UpdateNoteResponse ToUpdateResponse(this Note note)
    {
        return new UpdateNoteResponse
        {
            NoteId = note.Id,
            Title = note.Title,
            Content = note.Content,
            Significance = note.Significance,
            IsFavorite = note.IsFavorite,
            DateCreated = note.DateCreated,
            DateModified = note.DateModified,
            OwnerId = note.OwnerId,
            FolderId = note.FolderId
        };
    }

    public static NoteResponse ToNoteResponse(this Note note)
    {
        return new NoteResponse
        {
            NoteId = note.Id,
            Title = note.Title,
            Content = note.Content,
            Significance = note.Significance,
            IsFavorite = note.IsFavorite,
            DateCreated = note.DateCreated,
            DateModified = note.DateModified,
            OwnerId = note.OwnerId,
            FolderId = note.FolderId
        };
    }
}