using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace bookstore.api.Installers;

internal static class AuthInstaller
{
    public static IServiceCollection AddAuthInstaller(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opts =>
        {
            byte[] signingKeyBytes = Encoding.UTF8
                .GetBytes(configuration.GetSection("AuthOptions:SigningKey").Value);

            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration.GetSection("AuthOptions:Issuer").Value,
                ValidAudience = configuration.GetSection("AuthOptions:Audience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
            };
        });

        services.AddAuthorization();

        return services;
    }

    public static IApplicationBuilder UseAuthInstaller(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}