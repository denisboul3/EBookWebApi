using ISession = NHibernate.ISession;
using NHibernate;
using NHibernate.Criterion;
using bookstore.api.Models;
using bookstore.api.Repositories;
using NHibernate.Mapping;

namespace bookstore.api.necessary.Repositories.IRepositories.User;

public class UserRepository : RepositoryBase<UserModel>, IUserRepository
{
    public UserRepository(ISession session) : base(session) { }

    public void CreateUser(UserModel user)
    {
        Save(user);
    }

    public UserModel FindByLoginPassword(string login, string password)
    {
        return
            (UserModel)
            Session.CreateCriteria(typeof(UserModel))
                .Add(Restrictions.Eq("Login", login))
                .Add(Restrictions.Eq("Password", password))
                .SetCacheable(true)
                .SetCacheMode(CacheMode.Get)
                .UniqueResult()
            ;
    }
}