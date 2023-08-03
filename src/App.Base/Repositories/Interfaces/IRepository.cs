using System.Linq.Expressions;
using App.Base.ValueObjects;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace App.Base.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    /// <summary>
    /// Add entity
    /// </summary>
    /// <param name="entity">Entity name</param>
    void Add(T entity);

    /// <summary>
    /// Add entity asyncronously
    /// </summary>
    /// <param name="entity">Entity name</param>
    /// <returns>Void</returns>
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add list of entity
    /// </summary>
    /// <param name="entities">Entity list</param>
    void AddRange(IEnumerable<T> entities);

    /// <summary>
    /// Add list of entity asyncronously
    /// </summary>
    /// <param name="entities">Entity list</param>
    /// <returns>Void</returns>
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// CHeck if exists in database as per predicate
    /// </summary>
    /// <param name="predicate">Predicate defination</param>
    /// <returns>True or false</returns>
    bool Exists(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Check if exists in databse as per predicate asyncronously
    /// </summary>
    /// <param name="predicate">Predicate defination</param>
    /// <returns>True or false</returns>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    bool NotExists(Expression<Func<T, bool>> predicate);

    Task<bool> NotExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get entity
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="orderBy"></param>
    /// <param name="includeProperties"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="enableNoTracking"></param>
    /// <param name="ignoreQueryFilters"></param>
    /// <returns>Entity</returns>
    IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int? pageNumber = default, int? pageSize = default, bool enableNoTracking = true, bool ignoreQueryFilters = false);

    /// <summary>
    /// Get entity asyncronously
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="orderBy"></param>
    /// <param name="includeProperties"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="enableTracking"></param>
    /// <returns></returns>
    Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int? pageNumber = default(int?), int? pageSize = default(int?), bool enableTracking = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get entity by id
    /// </summary>
    /// <param name="id">Id of the entity</param>
    /// <returns>Entity</returns>
    T GetById(int id);

    /// <summary>
    /// Get eneity by id asyncronously
    /// </summary>
    /// <param name="id">Id of the entity</param>
    /// <returns>Entity</returns>
    Task<T> GetByIdAsync(long id);

    /// <summary>
    /// Get count of the rows in entity
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Count</returns>
    int GetCount(Expression<Func<T, bool>> filter = null);

    /// <summary>
    /// Get count of the rows in entity asyncronously
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Count</returns>
    Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get single row as per predicate
    /// </summary>
    /// <param name="predicate">Predicate defination</param>
    /// <returns>Entity</returns>
    T GetSingle(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Get single row as per predicate asyncronously
    /// </summary>
    /// <param name="predicate">Predicate defination</param>
    /// <returns>Entity</returns>
    Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// An instance of the data source control that was used to resolve the expression
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="orderBy"></param>
    /// <param name="includeProperties"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="enableNoTracking"></param>
    /// <param name="ignoreQueryFilters"></param>
    /// <returns>Entity</returns>
    IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int? pageNumber = default, int? pageSize = default, bool enableNoTracking = true, bool ignoreQueryFilters = false, bool? isActive = true);

    /// <summary>
    /// Delete records in bulk
    /// </summary>
    /// <param name="entity">Entity name</param>
    /// <param name="saveAndReturn">Save and return or not, false by default</param>
    /// <returns></returns>
    Task<List<T>> BulkDeleteAsync(List<T> entity, bool saveAndReturn = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Insert records in bulk
    /// </summary>
    /// <param name="entity">Entity name</param>
    /// <param name="saveAndReturn">Save and return or not, false by default</param>
    /// <returns></returns>
    Task<List<T>> BulkInsertAsync(List<T> entity, bool saveAndReturn = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update records in bulk
    /// </summary>
    /// <param name="entity">Entity name</param>
    /// <param name="saveAndReturn">Save and return or not, false by default</param>
    /// <returns></returns>
    Task<List<T>> BulkUpdateAsync(List<T> entity, bool saveAndReturn = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get tracked entries on a entity
    /// </summary>
    /// <returns>EntityEntry</returns>
    IEnumerable<EntityEntry<T>> GetTrackedEntries();

    /// <summary>
    /// Detach anything tracked in entity
    /// </summary>
    /// <param name="ignoreCurrentState">Ignore current state</param>
    void DetachTrackedEntries(bool ignoreCurrentState = false);

    /// <summary>
    /// Set entry state deleted of an entity
    /// </summary>
    /// <param name="entityToDelete"></param>
    void SetStateAsDeleted(T entityToDelete);

    /// <summary>
    /// Update different properties of entity
    /// </summary>
    /// <param name="entityToUpdate">Entity name to update</param>
    /// <param name="propertiesModified">Properties to modify with value</param>
    /// <param name="collectionPropertiesModified">Collection properties to modify with values</param>
    /// <param name="referencePropertiesModified">Reference properties to modify with values</param>
    /// <param name="collectionPropertiesNotModified">Collection properties not to modify with values</param>
    /// <param name="referencePropertiesNotModified">Reference properties not to modify with values</param>
    void Update(T entityToUpdate, string propertiesModified = "", string collectionPropertiesModified = "", string referencePropertiesModified = "", string collectionPropertiesNotModified = "", string referencePropertiesNotModified = "");

    /// <summary>
    /// Update entity properties taking dictionary parameter (key: string, value: object)
    /// </summary>
    /// <param name="id">Id of an object</param>
    /// <param name="modifiedProperties">Modified properties as dictionary</param>
    /// <returns></returns>
    Task<T> UpdateAsync(object id, Dictionary<string, object> modifiedProperties, CancellationToken cancellationToken = default);

    PagedResult<T> Paginate(IQueryable<T> queryable, int page = 1, int limit = 50);

    Task<PagedResult<T>> PaginateAsync(IQueryable<T> queryable, int page = 1, int limit = 50, CancellationToken cancellationToken = default);
}
