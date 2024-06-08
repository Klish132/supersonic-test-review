using Microsoft.AspNetCore.Http;

namespace core.Dto.Folder;

public record UpdateFolderRequest
{
    public required string Title { get; init; }
    public required IFormFile Image { get; init; }
}