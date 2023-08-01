using App.Installers.Interfaces;

namespace App.Configurations;

public static class ConfigureInstallers
{
    public static void InstallRequiredServices(this WebApplicationBuilder builder)
    {
        var installers = typeof(Program).Assembly.ExportedTypes
            .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IInstaller>();

        foreach (var installer in installers)
        {
            installer.InstallServices(builder);
        }
    }
}
