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
using UnitOfWork;

namespace bookstore.api.necessary.Mediator.Handlers.User;

public class ChangeUserPasswordHandler : ChangeUserPasswordProcessor, 
IRequestHandler<ChangeUserPasswordQuery, ResponseMessage<UserModel>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUoW _uOf;

    public ChangeUserPasswordHandler(IUserRepository userRepository, IUoW uOf)
    {
        _userRepository = userRepository;
        _uOf = uOf;
    }

    public async Task<ResponseMessage<UserModel>> Handle(ChangeUserPasswordQuery request, CancellationToken cancellationToken)
    {
        return await ChangeUserPassword(request);
    }

    private void CommitUserChangePassword(UserModel modifiedUser)
    {
        _userRepository.Save(modifiedUser);
        _uOf.Commit();
    }

    public async Task<ResponseMessage<UserModel>> ChangeUserPassword(ChangeUserPasswordQuery query)
    {
        var response = new ResponseMessage<UserModel>();

        try
        {
            var user = _userRepository.FindByLoginPassword(query.Login, query.OldPassword);

            if (user.IsNull())
            {
                response.PutError(ErrorCode.USER_CHANGE_PASSWORD, $"Wrong Credentials!");

                return await Task.FromResult(response);
            }

            user.Password = query.NewPassword;

            CommitUserChangePassword(user);

        }
        catch (Exception ex)
        {
            response.PutError(ErrorCode.USER_CHANGE_PASSWORD_EXCEPTION, $"{ex.Message} {ex.InnerException}");

            return await Task.FromResult(response);
        }

        return await Task.FromResult(response);
    }
}