using ISession = NHibernate.ISession;
using NHibernate;
using NHibernate.Criterion;
using bookstore.api.Models;
using bookstore.api.Repositories;
using NHibernate.Mapping;
namespace bookstore.api.necessary.Repositories.IRepositories.Price;

public class PriceRepository : RepositoryBase<PriceModel>, IPriceRepository
{
    public PriceRepository(ISession session) : base(session) { }

}