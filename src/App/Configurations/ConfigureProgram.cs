using App.Base.Configurations;
using App.User.Configurations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

namespace App.Configurations;

public static class ConfigureProgram
{
    /// <summary>
    /// Add services to the container
    /// </summary>
    /// <param name="builder">WebApplicationBuilder</param>
    public static void AddServicesToContainer(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services));

        builder.Services.AddControllersWithViews();

        builder.Services.AddBase();
        builder.Services.AddUser();

        builder.Services.AddAuthentication(
            CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(option =>
            {
                option.LoginPath = "/Auth/Login";
                option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });

        builder.InstallRequiredServices();
    }

    /// <summary>
    /// Configure the http request pipeline
    /// </summary>
    /// <param name="app">WebApplication</param>
    /// <returns>WebApplication</returns>
    public static WebApplication ConfigureHttpRequestPipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Auth}/{action=Login}/{id?}");

        return app;
    }
}
