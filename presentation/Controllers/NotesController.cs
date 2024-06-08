using core.Dto.Folder;
using core.Dto.Note;
using core.Exceptions.User;
using core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using services.abstractions.Interfaces;

namespace presentation.Controllers;


[ApiController]
[Authorize]
[Route("api/notes")]
public class NotesController : Controller
{
    private readonly IServicesManager _manager;
    
    public NotesController(IServicesManager manager)
    {
        _manager = manager;
    }
    
    [HttpPost]
    [ProducesResponseType<CreateNoteResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteRequest request)
    {
        if (!User.TryGetUserId(out var userId))
            throw new UserUnauthorizedHttpException();
        var response = await _manager.Notes.CreateNoteAsync(userId, request);
        return Ok(response);
    }
    
    [HttpPost("{noteId:guid}")]
    [ProducesResponseType<UpdateNoteResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateNote([FromRoute] Guid noteId, [FromBody] UpdateNoteRequest request)
    {
        if (!User.TryGetUserId(out var userId))
            throw new UserUnauthorizedHttpException();
        var response = await _manager.Notes.UpdateNoteAsync(userId, noteId, request);
        return Ok(response);
    }
    
    [HttpDelete("{noteId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteNote([FromRoute] Guid noteId)
    {
        if (!User.TryGetUserId(out var userId))
            throw new UserUnauthorizedHttpException();
        await _manager.Notes.DeleteNoteAsync(userId, noteId);
        return Ok();
    }
    
    [HttpGet("{noteId:guid}")]
    [ProducesResponseType<NoteResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNote([FromRoute] Guid noteId)
    {
        if (!User.TryGetUserId(out var userId))
            throw new UserUnauthorizedHttpException();
        var response = await _manager.Notes.GetNoteAsync(userId, noteId);
        return Ok(response);
    }
    
    [HttpGet]
    [ProducesResponseType<IEnumerable<NoteResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllNotes([FromQuery] GetNotesRequest request)
    {
        if (!User.TryGetUserId(out var userId))
            throw new UserUnauthorizedHttpException();
        var response = await _manager.Notes.GetUserNotesAsync(userId, request);
        return Ok(response);
    }
}