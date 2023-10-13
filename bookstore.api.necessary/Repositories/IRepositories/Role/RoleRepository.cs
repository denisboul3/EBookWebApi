using ISession = NHibernate.ISession;
using NHibernate;
using NHibernate.Criterion;
using bookstore.api.Models;
using bookstore.api.Repositories;
using NHibernate.Mapping;
using System.Drawing.Printing;

namespace bookstore.api.necessary.Repositories.IRepositories.Role;

public class RoleRepository : RepositoryBase<RoleModel>, IRoleRepository
{
    public RoleRepository(ISession session) : base(session) { }

    public void CreateRole(RoleModel role)
    {
        Save(role);
    }

    public RoleModel GetUserRole()
    {
        return
            (RoleModel)
            Session.CreateCriteria(typeof(RoleModel))
                .Add(Expression.Eq("Name", "User"))
                .SetCacheable(true)
                .SetCacheMode(CacheMode.Normal)
                .UniqueResult()
            ;
    }
}