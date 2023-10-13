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

public class CreateRoleHandler : CreateRoleProcessor, IRequestHandler<CreateRoleCommand, ResponseMessage<RoleModel>>
{
    private readonly IUoW _uOf;
    private readonly IRoleRepository _roleRepository;
    public CreateRoleHandler(IUoW uOf, IRoleRepository roleRepository)
    {
        _uOf = uOf;
        _roleRepository = roleRepository;
    }

    public async Task<ResponseMessage<RoleModel>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        return await CreateRole(request);
    }
    private void CommitCreateRole(RoleModel roleToBeSaved)
    {
        _roleRepository.Save(roleToBeSaved);
        _uOf.Commit();
    }

    public async Task<ResponseMessage<RoleModel>> CreateRole(CreateRoleCommand query)
    {
        var response = new ResponseMessage<RoleModel>();

        try
        {
            var roleToBeFound = _roleRepository.FindBy(query.Name, "Name");

            if (roleToBeFound.IsNull())
            {
                var newRole = new RoleModel();
                newRole.Name = query.Name;
                newRole.SetAudit(query.CreatedById);

                CommitCreateRole(newRole);
                response.Data = newRole;
            }
            else
            {
                response.PutError(ErrorCode.ROLE_ALREADY_EXISTS, $"Role with name '{query.Name}' already exists!");

                return await Task.FromResult(response);
            }
        }
        catch (Exception ex)
        {
            response.PutError(ErrorCode.ROLE_CREATE_EXCEPTION, $"{ex.Message} {ex.InnerException}");

            return await Task.FromResult(response);
        }

        return await Task.FromResult(response);
    }
}