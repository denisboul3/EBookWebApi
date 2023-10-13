using NHibernate.Dialect;
using NHibernate;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using ISession = NHibernate.ISession;
using UnitOfWork;
using NHibernate.Tool.hbm2ddl;
using bookstore.api.Models;
using bookstore.api.Mapping.DbMap;
using bookstore.api.Extensions;

namespace bookstore.api.Installers;

internal static class NHibernateInstaller
{
    public static void ConfigureNHibernateWithSqlServer(this IServiceCollection services, string connectionString)
    {
        try
        {
            var cfg = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2012
                        .ConnectionString(connectionString)
                        .Dialect<MsSql2012Dialect>()
                        .MaxFetchDepth(5)
                        .FormatSql()
                        .ShowSql()
                        .Raw("transaction.use_connection_on_system_prepare", "true")
                        .AdoNetBatchSize(100)
                    )
                    .Mappings(x =>
                    {
                        x.FluentMappings.AddFromAssemblyOf<UserMap>();
                        x.FluentMappings.AddFromAssemblyOf<RoleMap>();
                        x.FluentMappings.AddFromAssemblyOf<BookMap>();
                        x.FluentMappings.AddFromAssemblyOf<PriceMap>();
                    })
                    .Cache(c => c.UseSecondLevelCache().UseQueryCache()
                        .ProviderClass(typeof(NHibernate.Caches.RtMemoryCache.RtMemoryCacheProvider)
                            .AssemblyQualifiedName)
                    )
                    .CurrentSessionContext("web")
                    .BuildConfiguration();

            cfg.AddAssembly(Assembly.GetExecutingAssembly());

            var sessionFactory = cfg.BuildSessionFactory();

            services.AddSingleton<ISessionFactory>(sessionFactory);

            services.AddScoped<ISession>((ctx) =>
            {
                var sf = ctx.GetRequiredService<ISessionFactory>();

                return sf.WithOptions().OpenSession();

            });

            services.AddScoped<IUoW, UoWNh> ();

            var schemaExport = new SchemaExport(cfg);

            bool export = false;

            if (export)
            {
                schemaExport.Create(false, true);


                using (var session = sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var Roles = new List<string>() { "SU", "Admin", "User" };
                    foreach(var role in Roles)
                    { 
                        var tmpRole = session.Query<RoleModel>().FirstOrDefault(r => r.Name == role);
                        if (tmpRole == null)
                        {
                            tmpRole = new RoleModel
                            {
                                Name = role,
                                CreatedDate = DateTime.UtcNow,
                                CreatedBy = Guid.Empty
                            };
                            session.Save(tmpRole);
                        }
                    }

                    var suUser = session.Query<UserModel>().FirstOrDefault(u => u.Login == "SU");
                    var suRole = session.Query<RoleModel>().FirstOrDefault(r => r.Name == "SU");

                    if (suUser.IsNull() && !suRole.IsNull())
                    {
                        suUser = new UserModel
                        {
                            Login = "SU",
                            Password = "1234".ToSha3(),
                            Email = "su@su.com",
                            Role = suRole
                        };
                        session.Save(suUser);
                    }

                    transaction.Commit();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"NHibernate initialization failed.\nDetails: {ex.Message + ex.InnerException?.Message}");
        }
    }

    public static IServiceCollection AddCustomMvc(this IServiceCollection services)
    {
        // Add framework services.
        services.AddControllers()
          // Added for functional tests
          .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
          builder => builder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
        });

        return services;
    }

}
