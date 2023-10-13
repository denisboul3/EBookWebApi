using System;
using System.Transactions;
using bookstore.api.Extensions;
using NHibernate;
using ISession = NHibernate.ISession;

namespace UnitOfWork;

public class UoWNh : IUoW
{
    private readonly ISession _session;
    private ITransaction? _transaction;

    public UoWNh(ISession session) => _session = session;

    public void Commit()
    {
        try
        {
            if (!_session.IsNull())
            {
                _transaction = _session.BeginTransaction();

                _session.Flush();
                _transaction.Commit();
            }
        }
        catch
        {
            _transaction?.Rollback();
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }
}
