using core.Dto.Folder;
using core.Dto.Note;

namespace services.abstractions.Interfaces;

public interface IFoldersService
{
    public Task<CreateFolderResponse> CreateFolderAsync(Guid ownerId, CreateFolderRequest request);
    public Task<UpdateFolderResponse> UpdateFolderAsync(Guid ownerId, Guid folderId, UpdateFolderRequest request);
    public Task<IEnumerable<FolderResponse>> GetAllFoldersAsync(Guid ownerId);
    public Task<IEnumerable<NoteResponse>> GetFolderNotesAsync(Guid ownerId, Guid folderId, GetFolderNotesRequest request);
    public Task DeleteFolderAsync(Guid ownerId, Guid folderId);
    public Task<FolderResponse> GetFolderAsync(Guid ownerId, Guid folderId);
}