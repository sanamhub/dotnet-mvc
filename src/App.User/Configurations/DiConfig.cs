using App.User.Providers;
using App.User.Providers.Interfaces;
using App.User.Repositories;
using App.User.Repositories.Interfaces;
using App.User.Services;
using App.User.Services.Interfaces;
using App.User.Validators;
using App.User.Validators.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace App.User.Configurations;

public static class DiConfig
{
    public static void AddUser(this IServiceCollection services) => services
        .AddProviders()
        .AddRepositories()
        .AddServices()
        .AddValidators()
        ;

    public static IServiceCollection AddProviders(this IServiceCollection services) => services
        .AddScoped<IUserProvider, UserProvider>()
        ;

    public static IServiceCollection AddRepositories(this IServiceCollection services) => services
        .AddScoped<IUserRepository, UserRepository>()
        ;

    public static IServiceCollection AddServices(this IServiceCollection services) => services
        .AddScoped<IUserService, UserService>()
        ;

    public static IServiceCollection AddValidators(this IServiceCollection services) => services
        .AddScoped<IUserValidator, UserValidator>()
        ;
}
