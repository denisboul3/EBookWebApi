using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace bookstore.api.Installers;

internal static class Installer
{
    public static IServiceCollection AddCookiesService(this IServiceCollection services)
    {
        services.AddMemoryCache();

        services.AddResponseCaching();

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        return services;
    }

    public static IApplicationBuilder InstallCookiesAndHttpsRedirection(this IApplicationBuilder app)
    {
        app.UseResponseCaching();
        app.UseCookiePolicy();
        app.UseHttpsRedirection();

        return app;
    }
}