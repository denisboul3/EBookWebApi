using bookstore.api.Mediator.Queries;
using bookstore.api.Models;
using bookstore.api.ResponseMessage;

namespace bookstore.api.necessary.Mediator.Processors.User;

public interface GetUsersProcessor
{
    Task<ResponseMessage<IList<UserModel>>> GetAllUsers(GetUsersQuery query);
}