using core.Dto.Folder;
using domain.Entities;

namespace services.Extensions;

public static class FolderExtensions
{
    public static CreateFolderResponse ToCreateResponse(this Folder folder)
    {
        return new CreateFolderResponse
        {
            FolderId = folder.Id,
            Title = folder.Title,
            ImageId = Path.GetFileNameWithoutExtension(folder.ImagePath),
            OwnerId = folder.OwnerId
        };
    }

    public static UpdateFolderResponse ToUpdateResponse(this Folder folder)
    {
        return new UpdateFolderResponse
        {
            FolderId = folder.Id,
            Title = folder.Title,
            ImageId = Path.GetFileNameWithoutExtension(folder.ImagePath),
            OwnerId = folder.OwnerId
        };
    }

    public static FolderResponse ToFolderResponse(this Folder folder)
    {
        return new FolderResponse
        {
            FolderId = folder.Id,
            Title = folder.Title,
            ImageId = Path.GetFileNameWithoutExtension(folder.ImagePath),
            OwnerId = folder.OwnerId
        };
    }
}