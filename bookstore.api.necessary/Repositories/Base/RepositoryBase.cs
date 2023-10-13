using System.Collections.Generic;
using ISession = NHibernate.ISession;
using NHibernate;
using NHibernate.Criterion;
using bookstore.api.Models;
using Microsoft.AspNetCore.Http;

namespace bookstore.api.Repositories;

public abstract class RepositoryBase<T> where T : BaseModel
{
    protected ISession Session;

    protected RepositoryBase(ISession session)
    {
        Session = session;
    }

    public void Remove(T entity)
    {
        Session.Delete(entity);
    }

    public void Save(T entity)
    {
        Session.SaveOrUpdate(entity);
    }

    public T FindBy(Guid Id)
    {
        return
            (T)
            Session.CreateCriteria(typeof(T))
                .Add(Restrictions.Eq("Id", Id))
                .SetCacheable(true)
                .SetCacheMode(CacheMode.Get)
                .UniqueResult()
        ;
    }

    public T FindBy(string name, string property)
    {
        return
            (T)
            Session.CreateCriteria(typeof(T))
                .Add(Restrictions.Eq(property, name))
                .SetCacheable(true)
                .SetCacheMode(CacheMode.Get)
                .UniqueResult()
        ;
    }

    public IList<T> GetAll()
    {
        var criteriaQuery =
            Session.CreateCriteria(typeof(T))
                .SetCacheable(true)
                .SetCacheMode(CacheMode.Normal)
                .SetFlushMode(FlushMode.Never);

        return (List<T>)criteriaQuery.List<T>();
    }
}

public interface IRepository<T> where T : class
{
    void Save(T entity);
    void Remove(T entity);

    T FindBy(Guid Id);
    T FindBy(string name, string property);

    IList<T> GetAll();
}