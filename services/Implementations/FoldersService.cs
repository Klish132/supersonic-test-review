using core.Dto.Folder;
using core.Dto.Note;
using core.Enums;
using core.Exceptions.Folder;
using core.Extensions;
using domain.Entities;
using domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using services.abstractions.Interfaces;
using services.Extensions;

namespace services.Implementations;

internal class FoldersService : IFoldersService
{
    private readonly IRepositoryManager _manager;
    private readonly IConfiguration _config;

    public FoldersService(IRepositoryManager manager, IConfiguration config)
    {
        _manager = manager;
        _config = config;
    }

    public async Task<CreateFolderResponse> CreateFolderAsync(Guid ownerId, CreateFolderRequest request)
    {
        var old = await _manager.Folders.Query()
            .SingleOrDefaultAsync(f => f.OwnerId == ownerId && f.Title == request.Title);
        if (old != null)
            throw new FolderTitleConflictHttpException(request.Title);

        var filePath = await request.Image.SaveFormImage(_config["StoredFilesPath"]!);
        var folder = new Folder
        {
            Title = request.Title,
            ImagePath = filePath,
            OwnerId = ownerId
        };
        await _manager.Folders.CreateAsync(folder);
        await _manager.UnitOfWork.SaveChangesAsync();
        return folder.ToCreateResponse();
    }

    public async Task<UpdateFolderResponse> UpdateFolderAsync(Guid ownerId, Guid folderId, UpdateFolderRequest request)
    {
        var folder = await _manager.Folders.GetByIdAsync(folderId) ??
            throw new FolderNotFoundHttpException(folderId);
        if (folder.OwnerId != ownerId)
            throw new FolderActionForbiddenHttpException(ownerId, folderId);
        
        var old = await _manager.Folders.Query()
            .SingleOrDefaultAsync(f => f.OwnerId == ownerId && f.Title == request.Title);
        if (old != null)
            throw new FolderTitleConflictHttpException(request.Title);
        
        folder.Title = request.Title;
        var filePath = await request.Image.SaveFormImage(_config["StoredFilesPath"]!);
        folder.ImagePath = filePath;
        await _manager.Folders.UpdateAsync(folder);
        await _manager.UnitOfWork.SaveChangesAsync();
        return folder.ToUpdateResponse();
    }

    public async Task<IEnumerable<FolderResponse>> GetAllFoldersAsync(Guid ownerId)
    {
        return await _manager.Folders.Query()
            .Where(f => f.OwnerId == ownerId)
            .Select(f => f.ToFolderResponse()).ToListAsync();
    }

    public async Task<IEnumerable<NoteResponse>> GetFolderNotesAsync(Guid ownerId, Guid folderId,
        GetFolderNotesRequest request)
    {
        var folder = await _manager.Folders.GetByIdAsync(folderId) ??
                     throw new FolderNotFoundHttpException(folderId);
        if (folder.OwnerId != ownerId)
            throw new FolderActionForbiddenHttpException(ownerId, folderId);
        await _manager.Folders.LoadCollection(folder, f => f.Notes);
        var mult = request.SortOrder == NotesSortOrder.HighToLow ? -1 : 1;
        return folder.Notes.OrderBy(n => (int)n.Significance * mult)
            .Select(n => n.ToNoteResponse());
    }

    public async Task DeleteFolderAsync(Guid ownerId, Guid folderId)
    {
        var folder = await _manager.Folders.GetByIdAsync(folderId) ??
                     throw new FolderNotFoundHttpException(folderId);
        if (folder.OwnerId != ownerId)
            throw new FolderActionForbiddenHttpException(ownerId, folderId);
        await _manager.Folders.DeleteAsync(folder);
        await _manager.UnitOfWork.SaveChangesAsync();
    }

    public async Task<FolderResponse> GetFolderAsync(Guid ownerId, Guid folderId)
    {
        var folder = await _manager.Folders.GetByIdAsync(folderId) ??
                     throw new FolderNotFoundHttpException(folderId);
        if (folder.OwnerId != ownerId)
            throw new FolderActionForbiddenHttpException(ownerId, folderId);
        return folder.ToFolderResponse();
    }
}