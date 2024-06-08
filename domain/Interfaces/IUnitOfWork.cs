namespace domain.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}