using System.Linq.Expressions;
using CarWebMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace CarWebMVC.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext context;

    public GenericRepository(AppDbContext context)
    {
        this.context = context;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = context.Set<TEntity>().AsSplitQuery();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(object id, string includeProperties = "")
    {
        IQueryable<TEntity> query = context.Set<TEntity>().AsSplitQuery();

        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        var lambda = GetComparedKeyLambdaExpression(id);

        return await query.FirstOrDefaultAsync(lambda);
    }

    public virtual void Add(TEntity entity)
    {
        context.Add(entity);
    }

    public virtual void Update(TEntity entity)
    {
        context.Update(entity);
    }

    public virtual void Remove(TEntity entity)
    {
        context.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        context.RemoveRange(entities);
    }

    public async Task<bool> ExistsAsync(object id)
    {
        var lambdaExpression = GetComparedKeyLambdaExpression(id);
        return await context.Set<TEntity>().AnyAsync(lambdaExpression);
    }

    public virtual async Task LoadReferenceAsync(TEntity entity, string navigationProperty)
    {
        await context.Entry(entity).Reference(navigationProperty).LoadAsync();
    }

    public virtual async Task LoadCollectionAsync(TEntity entity, string navigationProperty)
    {
        await context.Entry(entity).Collection(navigationProperty).LoadAsync();
    }

    private Expression<Func<TEntity, bool>> GetComparedKeyLambdaExpression(object id)
    {
        var primaryKey = context.Model.FindEntityType(typeof(TEntity))?.FindPrimaryKey()?.Properties[0];
        if (primaryKey == null)
        {
            throw new InvalidOperationException("Primary key of TEntity not found.");
        }
        var parameter = Expression.Parameter(typeof(TEntity));
        var member = Expression.Property(parameter, primaryKey.Name);
        var constant = Expression.Constant(id);
        var equal = Expression.Equal(member, constant);
        return Expression.Lambda<Func<TEntity, bool>>(equal, parameter); // Represents TEntity => TEntity.Id == id
    }
}