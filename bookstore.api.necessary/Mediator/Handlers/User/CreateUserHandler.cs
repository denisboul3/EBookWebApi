using AutoMapper;
using ISession = NHibernate.ISession;
using UnitOfWork;
using bookstore.api.Models;
using MediatR;
using bookstore.api.Mediator.Queries;
using bookstore.api.necessary.Mediator.Processors.User;
using bookstore.api.necessary.Repositories.IRepositories.User;
using bookstore.api.necessary.Repositories.IRepositories.Role;
using bookstore.api.Extensions;
using bookstore.api.ResponseMessage;

namespace bookstore.api.necessary.Mediator.Handlers.User;

public class CreateUserHandler : CreateUserProcessor, IRequestHandler<CreateUserQuery, ResponseMessage<UserModel>>
{
    private readonly IUoW _uOf;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public CreateUserHandler(IUoW uOf, IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _uOf = uOf;
        _userRepository = userRepository;
       _roleRepository = roleRepository;
    }

    public async Task<ResponseMessage<UserModel>> Handle(CreateUserQuery request, CancellationToken cancellationToken)
    {
        return await CreateUser(request);
    }

    private void CommitCreateUser(UserModel createdUser)
    {
        _userRepository.Save(createdUser);
        _uOf.Commit();
    }

    public async Task<ResponseMessage<UserModel>> CreateUser(CreateUserQuery query)
    {
        var response = new ResponseMessage<UserModel>();

        try
        {
            var userToSearch = _userRepository.FindBy(query.Login, "Login") ?? _userRepository.FindBy(query.Email, "Email");

            if (!userToSearch.IsNull())
            {
                response.PutError(ErrorCode.USER_ALREADY_EXISTS, $"User with that credentials already exists!");

                return await Task.FromResult(response);
            }

            var userToBeCreated = new UserModel();

            userToBeCreated.Login = query.Login;
            userToBeCreated.Password = query.Password;
            userToBeCreated.Email = query.Email;

            var roleToBeInjected = _roleRepository.GetUserRole();

            if (roleToBeInjected.IsNull())
            {
                response.PutError(ErrorCode.CREATE_USER, $"Could not find the USER role. Database problem!");

                return await Task.FromResult(response);
            }

            userToBeCreated.SetRole(roleToBeInjected);

            CommitCreateUser(userToBeCreated);

            response.Data = userToBeCreated;
        }
        catch (Exception ex)
        {
            response.PutError(ErrorCode.CREATE_USER_EXCEPTION, $"{ex.Message} {ex.InnerException}");

            return await Task.FromResult(response);
        }

        return await Task.FromResult(response);
    }
}