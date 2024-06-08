using System.Linq.Expressions;
using core.Entities.Base;
using domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Implementations;

internal sealed class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<TEntity> _entities;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _entities = context.Set<TEntity>();
    }

    public async Task<bool> CheckExistsByIdAsync(Guid id)
    {
        var entity = await _entities.AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
        return entity != null;
    }

    public Task CreateAsync(TEntity entity)
    {
        _entities.Add(entity);
        return Task.CompletedTask;
    }

    public Task CreateManyAsync(IEnumerable<TEntity> entities)
    {
        _entities.AddRange(entities);
        return Task.CompletedTask;
    }

    public Task<TEntity?> GetByIdAsync(Guid id)
    {
        return _entities.AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
    }

    public Task UpdateAsync(TEntity entity)
    {
        _entities.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        _entities.Remove(entity);
        return Task.CompletedTask;
    }

    public IQueryable<TEntity> Query()
    {
        return _entities.AsQueryable().AsNoTracking();
    }

    public Task LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty?>> property) where TProperty : class
    {
        if (!_entities.Local.Contains(entity))
            _entities.Attach(entity);
        return _context.Entry(entity).Reference(property).LoadAsync();
    }

    public Task LoadCollection<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> property) where TProperty : class
    {
        if (!_entities.Local.Contains(entity))
            _entities.Attach(entity);
        return _context.Entry(entity).Collection(property).LoadAsync();
    }
}