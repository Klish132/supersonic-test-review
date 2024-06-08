using core.Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace domain.Entities;

public class User : IdentityUser<Guid>, IEntity
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryDate { get; set; }

    public ICollection<Folder> Folders { get; set; } = null!;
    public ICollection<Note> Notes { get; set; } = null!;
}