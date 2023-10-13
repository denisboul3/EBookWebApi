using bookstore.api.Models;
using bookstore.api.ResponseMessage;
using MediatR;

namespace bookstore.api.Mediator.Queries;

public record GetUsersQuery() : IRequest<ResponseMessage<IList<UserModel>>>;
public record CreateUserQuery(string Login, string Password, string Email) : IRequest<ResponseMessage<UserModel>>;
public record GetUserByLoginPasswordQuery(string Login, string Password) : IRequest<ResponseMessage<UserModel>>;
public record ChangeUserPasswordQuery(string Login, string OldPassword, string NewPassword) : IRequest<ResponseMessage<UserModel>>;
