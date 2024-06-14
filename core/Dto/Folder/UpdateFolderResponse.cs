namespace core.Dto.Folder;

public record UpdateFolderResponse
{
    public required Guid FolderId { get; init; }
    public required string Title { get; init; }
    public required string ImageId { get; init; }
    public required Guid OwnerId { get; init; }
}