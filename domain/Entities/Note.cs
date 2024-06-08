using core.Entities.Base;
using core.Enums;

namespace domain.Entities;

public class Note : IEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public Significance Significance { get; set; } = Significance.Common;
    public bool IsFavorite { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
    
    public Folder? Folder { get; set; }
    public Guid? FolderId { get; set; }
    public User Owner { get; set; } = null!;
    public Guid OwnerId { get; set; }
}