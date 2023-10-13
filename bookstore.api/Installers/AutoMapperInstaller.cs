using AutoMapper;
using bookstore.api.Mapping;

namespace bookstore.api.Installers;

internal static class AutoMapperInstaller
{
    public static IServiceCollection InstallMappings(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserModelToUserDtoMappingProfile>();
            cfg.AddProfile<BookModelToBookProjectionMappingProfile>();
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        return services;
    }
}