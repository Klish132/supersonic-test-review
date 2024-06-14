using core.Entities.Base;
using core.Enums;

namespace domain.Entities;

public class Folder : IEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string ImagePath { get; set; }

    public ICollection<Note> Notes { get; set; } = null!;
    public User Owner { get; set; } = null!;
    public Guid OwnerId { get; set; }
}