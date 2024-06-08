using domain.Entities;

namespace domain.Interfaces;

public interface IRepositoryManager
{
    IUnitOfWork UnitOfWork { get; }
    IRepository<User> Users { get; }
    IRepository<Folder> Folders { get; }
    IRepository<Note> Notes { get; }
}