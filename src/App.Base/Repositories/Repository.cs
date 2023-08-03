using System.Linq.Expressions;
using App.Base.Repositories.Interfaces;
using App.Base.ValueObjects;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace App.Base.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public void Add(T entity) => _dbSet.Add(entity);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default) => await _dbSet.AddAsync(entity, cancellationToken);

    public void AddRange(IEnumerable<T> entities) => _dbSet.AddRange(entities);

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default) => await _dbSet.AddRangeAsync(entities, cancellationToken);

    public bool Exists(Expression<Func<T, bool>> predicate) => GetQueryable().Any(predicate);

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => await GetQueryable(predicate).AnyAsync(cancellationToken);

    public bool NotExists(Expression<Func<T, bool>> predicate) => !GetQueryable().Any(predicate);

    public async Task<bool> NotExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => !await GetQueryable(predicate).AnyAsync(cancellationToken);

    public IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int? pageNumber = null, int? pageSize = null, bool enableNoTracking = true, bool ignoreQueryFilters = false)
    {
        return GetQueryable(filter, orderBy, includeProperties, pageNumber, pageSize, enableNoTracking, ignoreQueryFilters).ToList();
    }

    public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int? pageNumber = null, int? pageSize = null, bool enableTracking = false, CancellationToken cancellationToken = default)
    {
        return await GetQueryable(filter, orderBy, includeProperties, pageNumber, pageSize, enableTracking).ToListAsync(cancellationToken);
    }

    public T GetById(int id) => _dbSet.Find(id);

    public async Task<T> GetByIdAsync(long id) => await _dbSet.FindAsync(id);

    public int GetCount(Expression<Func<T, bool>> filter = null)
    {
        IQueryable<T> query = _dbSet;
        if (filter != null) { return query.Where(filter).Count(); }
        else { return query.Count(); }
    }

    public async Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;
        if (filter != null) { return await query.Where(filter).CountAsync(cancellationToken); }
        else { return await query.CountAsync(cancellationToken); }
    }

    public T GetSingle(Expression<Func<T, bool>> predicate) => GetBaseQueryable().FirstOrDefault(predicate);

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => await GetBaseQueryable().FirstOrDefaultAsync(predicate, cancellationToken);

    public IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int? pageNumber = default, int? pageSize = default, bool enableNoTracking = true, bool ignoreQueryFilters = false, bool? isActive = true)
    {
        IQueryable<T> query = enableNoTracking ? _dbSet.AsNoTracking() : _dbSet;

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        // todo : check why not working
        //if (isActive is not null)
        //{
        //    query = query.Where(x => x.IsActive == isActive);
        //}

        var propertiesToInclude = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var includeProperty in propertiesToInclude)
        {
            query = query.Include(includeProperty.Trim());
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (pageNumber != null && pageSize != null)
        {
            var skip = ((int)pageNumber - 1) * (int)pageSize;
            query = query.Skip(skip).Take((int)pageSize);
        }
        return query;
    }

    public async Task<List<T>> BulkDeleteAsync(List<T> entity, bool saveAndReturn = false, CancellationToken cancellationToken = default)
    {
        await _context.BulkDeleteAsync(entity, new BulkConfig { SetOutputIdentity = saveAndReturn }, cancellationToken: cancellationToken);
        return entity;
    }

    public async Task<List<T>> BulkInsertAsync(List<T> entity, bool saveAndReturn = false, CancellationToken cancellationToken = default)
    {
        await _context.BulkInsertAsync(entity, new BulkConfig { SetOutputIdentity = saveAndReturn }, cancellationToken: cancellationToken);
        return entity;
    }

    public async Task<List<T>> BulkUpdateAsync(List<T> entity, bool saveAndReturn = false, CancellationToken cancellationToken = default)
    {
        await _context.BulkUpdateAsync(entity, new BulkConfig { SetOutputIdentity = saveAndReturn }, cancellationToken: cancellationToken);
        return entity;
    }

    public IEnumerable<EntityEntry<T>> GetTrackedEntries()
    {
        return _context.ChangeTracker.Entries<T>();
    }

    public virtual void DetachTrackedEntries(bool ignoreCurrentState = false)
    {
        foreach (var entry in GetTrackedEntries().ToList())
        {
            if (ignoreCurrentState || entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                entry.State = EntityState.Detached;
        }
    }

    public void SetStateAsDeleted(T entityToDelete)
    {
        var entity = _context.Entry(entityToDelete);
        entity.State = EntityState.Deleted;
    }

    public void Update(T entityToUpdate, string propertiesModified = "", string collectionPropertiesModified = "", string referencePropertiesModified = "", string collectionPropertiesNotModified = "", string referencePropertiesNotModified = "")
    {
        var entity = _context.Entry(entityToUpdate);
        entity.State = EntityState.Modified;
        foreach (var property in propertiesModified.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            _context.Entry(entityToUpdate).Property(property.Trim()).IsModified = true;
        }

        foreach (var property in referencePropertiesModified.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            _context.Entry(entityToUpdate).Reference(property.Trim()).IsModified = true;
        }

        foreach (var property in referencePropertiesNotModified.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            _context.Entry(entityToUpdate).Property(property.Trim()).IsModified = false;
        }

        foreach (var property in collectionPropertiesModified.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            _context.Entry(entityToUpdate).Collection(property.Trim()).IsModified = true;
        }

        foreach (var property in collectionPropertiesNotModified.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            _context.Entry(entityToUpdate).Collection(property.Trim()).IsModified = false;
        }
    }

    public async Task<T> UpdateAsync(object id, Dictionary<string, object> modifiedProperties, CancellationToken cancellationToken = default)
    {
        var theEntry = _dbSet.Find(id) ?? throw new Exception("Record not found.");
        foreach (var pair in modifiedProperties)
        {
            var theProp = _context.Entry(theEntry).Property(pair.Key);
            theProp.CurrentValue = pair.Value;
        }
        await _context.SaveChangesAsync(cancellationToken);
        return theEntry;
    }

    public PagedResult<T> Paginate(IQueryable<T> queryable, int page = 1, int limit = 50)
    {
        return new PagedResult<T>(
            queryable.Skip((page - 1) * limit).Take(limit).ToList(),
            queryable.Count(),
            page,
            limit
        );
    }

    public async Task<PagedResult<T>> PaginateAsync(IQueryable<T> queryable, int page = 1, int limit = 50, CancellationToken cancellationToken = default)
    {
        return new PagedResult<T>(
            await queryable.Skip((page - 1) * limit).Take(limit).ToListAsync(cancellationToken),
             await queryable.CountAsync(cancellationToken),
            page,
            limit
        );
    }

    #region Private methods

    private IQueryable<T> GetBaseQueryable() => _dbSet;

    #endregion
}
