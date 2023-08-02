using App.Base.Entities;
using App.Extensions;
using App.User.Configurations;
using Microsoft.EntityFrameworkCore;

namespace App.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddUser();

        modelBuilder.AddGlobalHasQueryFilterForBaseTypeEntities<BaseAuditableEntity>(x => x.IsActive);

        modelBuilder.ConvertToSnakeCase();

        base.OnModelCreating(modelBuilder);
    }
}
