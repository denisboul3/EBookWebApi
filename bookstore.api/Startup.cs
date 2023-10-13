using bookstore.api.Installers;
using System.Reflection;
using bookstore.api.Mapping;
using bookstore.api.necessary.Mediator.Handlers.User;
using System.Text.Json.Serialization;

public class Startup
{
    public IConfiguration Configuration { get; }
    public IWebHostEnvironment HostEnv { get; }

    public Startup(IConfiguration configuration, IWebHostEnvironment hostEnv)
    {
        this.Configuration = configuration;
        this.HostEnv = hostEnv;
    }

    public void Configureservices(IServiceCollection services)
    {
        services.AddCookiesService();

        services.AddSwaggerInstaller();
        
        services.AddAuthInstaller(this.Configuration);

        services.AddRoutingVersioningInstaller();

        services.InstallMappings();

        services.ConfigureValidators();

        services.SetUpRepositories();
        services.SetUpMediator();

        services.ConfigureNHibernateWithSqlServer(this.Configuration.GetConnectionString("DefaultSQLConnection"));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            typeof(GetUsersHandler).GetTypeInfo().Assembly)
        );

        services.AddCustomMvc();

        services.AddHttpClient();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseAuthInstaller();
        app.InstallCookiesAndHttpsRedirection();
        app.Use(async (context, next) =>
        {
            if (context.Request.Method == HttpMethods.Options)
            {
                context.Response.StatusCode = 200;
                context.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:4200");
                context.Response.Headers.Add("Access-Control-Allow-Methods", "GET,HEAD,OPTIONS,POST,PUT,DELETE");
                context.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, Authorization");
                context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                await context.Response.WriteAsync("OK");
            }
            else
            {
                await next();
            }
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();
        });

        app.UseSwaggerInstaller();


        app.UseRoutingVersioningInstaller();

    }
}