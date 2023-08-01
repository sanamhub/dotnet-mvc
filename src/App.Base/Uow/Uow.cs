using App.Base.Uow.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.Base.Uow;

internal class Uow : IUow
{
    public DbContext Context { get; }

    public Uow(DbContext context) => Context = context;

    public void Add<T>(T t) where T : class => Context.Set<T>().Add(t);

    public async Task AddAsync<T>(T t, CancellationToken cancellationToken = default) where T : class => await Context.Set<T>().AddAsync(t, cancellationToken);

    public void AddRange<T>(IEnumerable<T> t) where T : class => Context.Set<T>().AddRange(t);

    public async Task AddRangeAsync<T>(IEnumerable<T> t, CancellationToken cancellationToken = default) where T : class => await Context.Set<T>().AddRangeAsync(t, cancellationToken);

    public void Remove<T>(T t) where T : class => Context.Set<T>().Remove(t);

    public void RemoveRange<T>(IEnumerable<T> t) where T : class => Context.Set<T>().RemoveRange(t);

    public void SaveChanges() => Context.SaveChanges();

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) => await Context.SaveChangesAsync(cancellationToken);
}
