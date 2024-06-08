using System.Linq.Expressions;

namespace domain.Interfaces;

public interface ITypedRepository<TKey, TEntity>
{
    Task<bool> CheckExistsByIdAsync(TKey id);
    Task CreateAsync(TEntity entity);
    Task CreateManyAsync(IEnumerable<TEntity> entities);
    Task<TEntity?> GetByIdAsync(TKey id);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    IQueryable<TEntity> Query();
    Task LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty?>> property)
        where TProperty : class;
    Task LoadCollection<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> property)
        where TProperty : class;
}