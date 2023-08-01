using App.Data;
using App.Installers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.Installers;

public class AppDbContextInstaller : IInstaller
{
    public void InstallServices(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration["ConnectionStrings:DefaultConnection"]);
        });

        builder.Services.AddScoped<DbContext, AppDbContext>();
    }
}
