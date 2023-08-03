using Microsoft.EntityFrameworkCore;

namespace App.User.Configurations;
public static class EntityRegistrar
{
    public static ModelBuilder AddUser(this ModelBuilder builder)
    {
        builder.Entity<Entities.User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        return builder;
    }
}
