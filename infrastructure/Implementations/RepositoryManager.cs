using domain.Entities;
using domain.Interfaces;

namespace infrastructure.Implementations;

internal class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IRepository<User>> _usersRepo;
    private readonly Lazy<IRepository<Folder>> _foldersRepo;
    private readonly Lazy<IRepository<Note>> _notesRepo;
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public IUnitOfWork UnitOfWork => _unitOfWork.Value;
    public IRepository<User> Users => _usersRepo.Value;
    public IRepository<Folder> Folders => _foldersRepo.Value;
    public IRepository<Note> Notes => _notesRepo.Value;
    
    public RepositoryManager(ApplicationDbContext context)
    {
        _usersRepo = new Lazy<IRepository<User>>(() => new Repository<User>(context));
        _foldersRepo = new Lazy<IRepository<Folder>>(() => new Repository<Folder>(context));;
        _notesRepo = new Lazy<IRepository<Note>>(() => new Repository<Note>(context));;
        _unitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(context));;
    }
}