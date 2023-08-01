using App.Base.Configurations;
using App.User.Configurations;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace App.Configurations;

public static class ConfigureProgram
{
    /// <summary>
    /// Add services to the container
    /// </summary>
    /// <param name="builder">WebApplicationBuilder</param>
    public static void AddServicesToContainer(this WebApplicationBuilder builder)
    {
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
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
