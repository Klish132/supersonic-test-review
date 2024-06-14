using core.Enums;

namespace core.Dto.Folder;

public record GetFolderNotesRequest
{
    public NotesSortOrder SortOrder { get; init; } = NotesSortOrder.HighToLow;
}