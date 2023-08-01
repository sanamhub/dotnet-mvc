namespace App.Installers.Interfaces;

public interface IInstaller
{
    void InstallServices(WebApplicationBuilder builder);
}
