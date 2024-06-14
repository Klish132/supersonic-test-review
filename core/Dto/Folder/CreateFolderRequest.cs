using Microsoft.AspNetCore.Http;

namespace core.Dto.Folder;

public record CreateFolderRequest
{
    public required string Title { get; init; }
    public required IFormFile Image { get; init; }
}