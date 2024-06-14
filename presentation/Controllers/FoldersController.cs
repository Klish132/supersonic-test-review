using core.Dto.Folder;
using core.Dto.Note;
using core.Dto.User;
using core.Exceptions.User;
using core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using services.abstractions.Interfaces;

namespace presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/folders")]
public class FoldersController : Controller
{
    private readonly IServicesManager _manager;
    
    public FoldersController(IServicesManager manager)
    {
        _manager = manager;
    }
    
    [HttpPost]
    [ProducesResponseType<CreateFolderResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateFolder([FromForm] CreateFolderRequest request)
    {
        if (!User.TryGetUserId(out var userId))
            throw new UserUnauthorizedHttpException();
        var response = await _manager.Folders.CreateFolderAsync(userId, request);
        return Ok(response);
    }
    
    [HttpPost("{folderId:guid}")]
    [ProducesResponseType<UpdateFolderResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateFolder([FromRoute] Guid folderId, [FromForm] UpdateFolderRequest request)
    {
        if (!User.TryGetUserId(out var userId))
            throw new UserUnauthorizedHttpException();
        var response = await _manager.Folders.UpdateFolderAsync(userId, folderId, request);
        return Ok(response);
    }
    
    [HttpDelete("{folderId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteFolder([FromRoute] Guid folderId)
    {
        if (!User.TryGetUserId(out var userId))
            throw new UserUnauthorizedHttpException();
        await _manager.Folders.DeleteFolderAsync(userId, folderId);
        return Ok();
    }
    
    [HttpGet]
    [ProducesResponseType<IEnumerable<FolderResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllFolders()
    {
        if (!User.TryGetUserId(out var userId))
            throw new UserUnauthorizedHttpException();
        var response = await _manager.Folders.GetAllFoldersAsync(userId);
        return Ok(response);
    }
    
    [HttpGet("{folderId:guid}")]
    [ProducesResponseType<FolderResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFolder([FromRoute] Guid folderId)
    {
        if (!User.TryGetUserId(out var userId))
            throw new UserUnauthorizedHttpException();
        var response = await _manager.Folders.GetFolderAsync(userId, folderId);
        return Ok(response);
    }
    
    [HttpGet("{folderId:guid}/notes")]
    [ProducesResponseType<IEnumerable<NoteResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFolderNotes([FromRoute] Guid folderId, [FromQuery] GetFolderNotesRequest request)
    {
        if (!User.TryGetUserId(out var userId))
            throw new UserUnauthorizedHttpException();
        var response = await _manager.Folders.GetFolderNotesAsync(userId, folderId, request);
        return Ok(response);
    }
}