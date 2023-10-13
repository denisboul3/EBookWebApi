using bookstore.api.Mediator.Queries;
using bookstore.api.Models;
using bookstore.api.ResponseMessage;

namespace bookstore.api.necessary.Mediator.Processors.User;

public interface CreateUserProcessor
{
    Task<ResponseMessage<UserModel>> CreateUser(CreateUserQuery query);
}