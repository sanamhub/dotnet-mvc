using App.Base.Uow.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace App.Base.Configurations;

public static class DiConfig
{
    public static void AddBase(this IServiceCollection services) => services
            .ConfigureUow()
            ;

    private static IServiceCollection ConfigureUow(this IServiceCollection services) => services
        .AddScoped<IUow, Uow.Uow>()
        ;
}
