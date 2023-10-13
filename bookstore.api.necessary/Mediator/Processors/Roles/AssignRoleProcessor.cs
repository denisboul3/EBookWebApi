using bookstore.api.Mediator.Queries;
using bookstore.api.Models;
using bookstore.api.ResponseMessage;

namespace bookstore.api.necessary.Mediator.Processors.Roles;

public interface AssignRoleProcessor
{
    Task<ResponseMessage<UserModel>> AssignRole(AssignRoleCommand query);
}