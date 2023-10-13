using Microsoft.OpenApi.Models;

namespace bookstore.api.Installers;

internal static class SwaggerInstaller
{
    public static IServiceCollection AddSwaggerInstaller(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "E-Book Store - HTTP API",
                Version = "v1",
                Description = "The Catalog Microservice HTTP API for E-Book Store service",
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description =
                "Authorization: Bearer {token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerInstaller(this IApplicationBuilder app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.EnableTryItOutByDefault();
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Book Store - HTTP API");
        });

        return app;
    }
}