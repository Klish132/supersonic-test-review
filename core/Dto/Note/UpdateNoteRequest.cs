using core.Enums;

namespace core.Dto.Note;

public record UpdateNoteRequest
{
    public required string Title { get; init; }
    public required string Content { get; init; }
    public required Significance Significance { get; init; } = Significance.Common;
    public required bool IsFavorite { get; init; }
    public required Guid? FolderId { get; init; }
}