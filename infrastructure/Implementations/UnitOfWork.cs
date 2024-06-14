using domain.Interfaces;

namespace infrastructure.Implementations;

internal class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}