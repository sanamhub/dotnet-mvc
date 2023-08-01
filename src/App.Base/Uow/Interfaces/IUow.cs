using Microsoft.EntityFrameworkCore;

namespace App.Base.Uow.Interfaces;

internal interface IUow
{
    DbContext Context { get; }

    void Add<T>(T t) where T : class;

    Task AddAsync<T>(T t, CancellationToken cancellationToken = default) where T : class;

    void AddRange<T>(IEnumerable<T> t) where T : class;

    Task AddRangeAsync<T>(IEnumerable<T> t, CancellationToken cancellationToken = default) where T : class;

    void Remove<T>(T t) where T : class;

    void RemoveRange<T>(IEnumerable<T> t) where T : class;

    void SaveChanges();

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
