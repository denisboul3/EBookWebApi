using ISession = NHibernate.ISession;
using NHibernate;
using NHibernate.Criterion;
using bookstore.api.Models;
using bookstore.api.Repositories;
using NHibernate.Mapping;
using System.Drawing.Printing;

namespace bookstore.api.necessary.Repositories.IRepositories.Book;

public class BookRepository : RepositoryBase<BookModel>, IBookRepository
{
    public BookRepository(ISession session) : base(session) { }

}