using AutoMapper;
using ISession = NHibernate.ISession;
using UnitOfWork;
using bookstore.api.Models;
using MediatR;
using bookstore.api.Mediator.Queries;
using bookstore.api.necessary.Mediator.Processors.Roles;
using bookstore.api.necessary.Repositories.IRepositories.User;
using bookstore.api.necessary.Repositories.IRepositories.Role;
using bookstore.api.ResponseMessage;
using bookstore.api.Extensions;

namespace bookstore.api.necessary.Mediator.Handlers.Roles;

public class AssignRoleHandler : AssignRoleProcessor, IRequestHandler<AssignRoleCommand, ResponseMessage<UserModel>>
{
    private readonly IUoW _uOf;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;

    public AssignRoleHandler(IUoW uOf, IRoleRepository roleRepository, IUserRepository userRepository)
    {
        _uOf = uOf;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
    }

    public async Task<ResponseMessage<UserModel>> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        return await AssignRole(request);
    }

    private void CommitAssignRole(UserModel userToBeSaved)
    {
        _userRepository.Save(userToBeSaved);
        _uOf.Commit();
    }

    public async Task<ResponseMessage<UserModel>> AssignRole(AssignRoleCommand query)
    {
        var response = new ResponseMessage<UserModel>();

        try
        {
            var userToBeAssigned = _userRepository.FindBy(query.UserId);

            if (!userToBeAssigned.IsNull())
            {
                var roleToBeFound = _roleRepository.FindBy(query.RoleId);
                if (!roleToBeFound.IsNull())
                {
                    userToBeAssigned.SetRole(roleToBeFound);
                    CommitAssignRole(userToBeAssigned);

                    response.Data = userToBeAssigned;
                }
                else
                {
                    response.PutError(ErrorCode.USER_ASSIGN_ROLE_NOT_FOUND, $"Role with ID {query.RoleId} was not found");

                    return await Task.FromResult(response);
                }
            } 
            else
            {
                response.PutError(ErrorCode.USER_NOT_FOUND, $"User with ID {query.UserId} was not found");

                return await Task.FromResult(response);
            }
        }
        catch (Exception ex)
        {
            response.PutError(ErrorCode.ROLE_ASSIGN_EXCEPTION, $"{ex.Message} {ex.InnerException}");

            return await Task.FromResult(response);
        }

        return await Task.FromResult(response);
    }
}