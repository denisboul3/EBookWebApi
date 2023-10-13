using AutoMapper;
using MediatR;
using ISession = NHibernate.ISession;
using bookstore.api.Mediator.Queries;
using bookstore.api.Models;
using bookstore.api.necessary.Mediator.Processors.User;
using bookstore.api.ResponseMessage;
using bookstore.api.necessary.Repositories.IRepositories.User;

namespace bookstore.api.necessary.Mediator.Handlers.User;

public class GetUsersHandler : GetUsersProcessor, IRequestHandler<GetUsersQuery, ResponseMessage<IList<UserModel>>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ResponseMessage<IList<UserModel>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await GetAllUsers(request);
    }

    public async Task<ResponseMessage<IList<UserModel>>> GetAllUsers(GetUsersQuery query)
    {
        var response = new ResponseMessage<IList<UserModel>>();

        try
        {
            var users = _userRepository.GetAll();

            response.Data = users;
        }
        catch (Exception ex)
        {
            response.PutError(ErrorCode.USER_RETRIEVE_EXCEPTION, $"{ex.Message} {ex.InnerException}");

            return await Task.FromResult(response);
        }

        return await Task.FromResult(response);
    }
}