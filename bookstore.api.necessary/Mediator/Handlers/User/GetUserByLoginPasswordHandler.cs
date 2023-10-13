using AutoMapper;
using MediatR;
using NHibernate.Criterion;
using ISession = NHibernate.ISession;
using bookstore.api.Mediator.Queries;
using bookstore.api.Models;
using bookstore.api.Extensions;
using bookstore.api.necessary.Mediator.Processors.User;
using bookstore.api.necessary.Repositories.IRepositories.User;
using bookstore.api.ResponseMessage;

namespace bookstore.api.necessary.Mediator.Handlers.User;

public class GetUserByLoginPasswordHandler : GetUserByLoginPasswordProcessor, 
    IRequestHandler<GetUserByLoginPasswordQuery, ResponseMessage<UserModel>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByLoginPasswordHandler(IMapper mapper, IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ResponseMessage<UserModel>> Handle(GetUserByLoginPasswordQuery request, CancellationToken cancellationToken)
    {
        return await GetUserByLoginPassword(request);
    }

    public async Task<ResponseMessage<UserModel>> GetUserByLoginPassword(GetUserByLoginPasswordQuery query)
    {
        var response = new ResponseMessage<UserModel>();

        try
        {
            var user = _userRepository.FindByLoginPassword(query.Login, query.Password);

            if (user.IsNull())
            {
                response.PutError(ErrorCode.USER_NOT_FOUND, $"Could not find any account with the credentials you entered!");

                return await Task.FromResult(response);
            }

            response.Data = user;
        }
        catch (Exception ex)
        {
            response.PutError(ErrorCode.USER_LOGIN_EXCEPTION, $"{ex.Message} {ex.InnerException}");

            return await Task.FromResult(response);
        }

        return await Task.FromResult(response);
    }
}