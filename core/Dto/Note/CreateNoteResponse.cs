using core.Enums;

namespace core.Dto.Note;

public record CreateNoteResponse
{
    public required Guid NoteId { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
    public required Significance Significance { get; init; }
    public required bool IsFavorite { get; init; }
    public required DateTime DateCreated { get; init; }
    public required DateTime DateModified { get; init; }
    public required Guid OwnerId { get; init; }
    public Guid? FolderId { get; init; }
}