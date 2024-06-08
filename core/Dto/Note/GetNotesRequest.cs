using Microsoft.AspNetCore.Mvc;

namespace core.Dto.Note;

public record GetNotesRequest
{
    [FromQuery(Name="search")]
    public string? SearchText { get; init; }
}