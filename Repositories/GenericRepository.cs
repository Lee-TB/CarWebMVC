using System.Linq.Expressions;
using CarWebMVC.Data;
using CarWebMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWebMVC.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext context;

    public GenericRepository(AppDbContext context)
    {
        this.context = context;
    }
    public async Task<IEnumerable<TEntity>> GetAsync()
    {
        return await context.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(
        string[]? includeProperties = null,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null
        )
    {
        IQueryable<TEntity> query = context.Set<TEntity>().AsSplitQuery();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.AsNoTracking().ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(
        string? includeProperties = null,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null
        )
    {
        IQueryable<TEntity> query = context.Set<TEntity>().AsSplitQuery();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.AsNoTracking().ToListAsync();
    }

    public virtual async Task<PaginatedList<TEntity>> GetPaginatedAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        int pageIndex = 1, int pageSize = 10
    )
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

        if (orderBy == null)
        {
            var primaryKey = context.Model.FindEntityType(typeof(TEntity))?.FindPrimaryKey()?.Properties[0];
            if (primaryKey is not null)
            {
                orderBy = query => query.OrderBy(entity => EF.Property<object>(entity, primaryKey.Name));
                query = orderBy(query);
            }
        }
        else if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await PaginatedList<TEntity>.CreateAsync(query.AsNoTracking(), pageIndex, pageSize);
    }

    public virtual async Task<TEntity?> GetByIdAsync(object id, string includeProperties = "")
    {
        IQueryable<TEntity> query = context.Set<TEntity>().AsSplitQuery();

        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        var lambda = GetComparedKeyLambdaExpression(id);

        return await query.AsNoTracking().FirstOrDefaultAsync(lambda);
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