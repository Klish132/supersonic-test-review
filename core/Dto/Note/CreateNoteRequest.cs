using core.Enums;

namespace core.Dto.Note;

public record CreateNoteRequest
{
    public required string Title { get; init; }
    public required string Content { get; init; }
    public required Significance Significance { get; init; } = Significance.Common;
    public required bool IsFavorite { get; init; }
    public Guid? FolderId { get; init; }
}