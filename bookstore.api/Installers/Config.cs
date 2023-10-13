using bookstore.api.DTO;
using bookstore.api.necessary.Mediator.Handlers.Roles;
using bookstore.api.necessary.Mediator.Handlers.User;
using bookstore.api.necessary.Mediator.Processors.Book;
using bookstore.api.necessary.Mediator.Processors.Roles;
using bookstore.api.necessary.Mediator.Processors.User;
using bookstore.api.necessary.Repositories.IRepositories.Book;
using bookstore.api.necessary.Repositories.IRepositories.Price;
using bookstore.api.necessary.Repositories.IRepositories.Role;
using bookstore.api.necessary.Repositories.IRepositories.User;
using bookstore.api.necessary.Validators;
using FluentValidation;

namespace bookstore.api.Installers;

internal static class CConfigureProcessors
{
    public static IServiceCollection SetUpRepositories(this IServiceCollection services)
    {
        services.AddScoped<GetUsersProcessor, GetUsersHandler>();
        services.AddScoped<GetUserByLoginPasswordProcessor, GetUserByLoginPasswordHandler>();
        services.AddScoped<CreateUserProcessor, CreateUserHandler>();

        services.AddScoped<CreateRoleProcessor, CreateRoleHandler>();
        services.AddScoped<AssignRoleProcessor, AssignRoleHandler>();


        services.AddScoped<AddBookPriceProcessor, AddBookPriceHandler>();
        services.AddScoped<EditBookPriceProcessor, EditBookPriceHandler>();
        services.AddScoped<EditBookProcessor, EditBookHandler>();
        services.AddScoped<CreateBookProcessor, CreateBookHandler>();
        services.AddScoped<GetBookInfoProcessor, GetBookInfoHandler>();
        services.AddScoped<GetAllBooksProcessor, GetAllBooksHandler>();
        services.AddScoped<DeleteBookProcessor, DeleteBookHandler>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IPriceRepository, PriceRepository>();
        return services;
    }
    public static IServiceCollection SetUpMediator(this IServiceCollection services)
    {
        // User-Related
        services.AddScoped<GetUsersProcessor, GetUsersHandler>();
        services.AddScoped<GetUserByLoginPasswordProcessor, GetUserByLoginPasswordHandler>();
        services.AddScoped<CreateUserProcessor, CreateUserHandler>();

        // Role-Related
        services.AddScoped<CreateRoleProcessor, CreateRoleHandler>();
        services.AddScoped<AssignRoleProcessor, AssignRoleHandler>();

        // Book-Related
        services.AddScoped<AddBookPriceProcessor, AddBookPriceHandler>();
        services.AddScoped<EditBookPriceProcessor, EditBookPriceHandler>();
        services.AddScoped<EditBookProcessor, EditBookHandler>();
        services.AddScoped<CreateBookProcessor, CreateBookHandler>();
        services.AddScoped<GetBookInfoProcessor, GetBookInfoHandler>();
        services.AddScoped<GetAllBooksProcessor, GetAllBooksHandler>();
        services.AddScoped<DeleteBookProcessor, DeleteBookHandler>();

        return services;
    }

    internal static IServiceCollection ConfigureValidators(this IServiceCollection services)
    {
        services.AddTransient<IValidator<CreateUserDto>, RegistrationValidator>();

        return services;
    }
}