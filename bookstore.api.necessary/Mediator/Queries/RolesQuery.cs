using bookstore.api.ResponseMessage;
using bookstore.api.DTO;
using bookstore.api.Models;
using MediatR;

namespace bookstore.api.Mediator.Queries;

public record CreateRoleCommand(Guid CreatedById, string Name) : IRequest<ResponseMessage<RoleModel>>;
public record AssignRoleCommand(Guid CreatedById, Guid RoleId, Guid UserId) : IRequest<ResponseMessage<UserModel>>;