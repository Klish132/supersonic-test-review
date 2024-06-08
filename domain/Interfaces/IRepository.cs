namespace domain.Interfaces;

public interface IRepository<TEntity> : ITypedRepository<Guid, TEntity>
{
    
}