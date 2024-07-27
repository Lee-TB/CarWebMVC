using System.Linq.Expressions;
using CarWebMVC.Models;

namespace CarWebMVC.Repositories;

public interface IGenericRepository<TEntity>
{
    public Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");
    public Task<PaginatedList<TEntity>> GetPaginatedAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "", int pageIndex = 1, int pageSize = 10);
    public Task<TEntity?> GetByIdAsync(object id, string includeProperties = "");
    public void Add(TEntity entity);
    public void Update(TEntity entity);
    public void Remove(TEntity entity);
    public void RemoveRange(IEnumerable<TEntity> entities);
    public Task<bool> ExistsAsync(object id);
    public Task LoadReferenceAsync(TEntity entity, string navigationProperty);
    public Task LoadCollectionAsync(TEntity entity, string navigationProperty);
}