using AutoMapper;
using ISession = NHibernate.ISession;
using UnitOfWork;
using bookstore.api.Models;
using MediatR;
using bookstore.api.Mediator.Queries;
using bookstore.api.ResponseMessage;
using bookstore.api.necessary.Mediator.Processors.Book;
using bookstore.api.necessary.Repositories.IRepositories.Book;
using Org.BouncyCastle.Asn1.Esf;
using bookstore.api.Extensions;

namespace bookstore.api.necessary.Mediator.Handlers.User;

public class CreateBookHandler : CreateBookProcessor, IRequestHandler<CreateBookQuery, ResponseMessage<BookModel>>
{
    private readonly IUoW _uOf;
    private readonly IBookRepository _bookRepository;

    public CreateBookHandler(IUoW uOf, IBookRepository bookRepository)
    {
        _uOf = uOf;
        _bookRepository = bookRepository;
    }

    public async Task<ResponseMessage<BookModel>> Handle(CreateBookQuery request, CancellationToken cancellationToken)
    {
        return await CreateBook(request);
    }

    private void CommitCreateBook(BookModel createdBook)
    {
        _bookRepository.Save(createdBook);
        _uOf.Commit();
    }

    public async Task<ResponseMessage<BookModel>> CreateBook(CreateBookQuery query)
    {
        var response = new ResponseMessage<BookModel>();
        try
        {
            var bookToSearch = _bookRepository.FindBy(query.book.Name, "Name");

            if (!bookToSearch.IsNull())
            {
                response.PutError(ErrorCode.BOOK_ALREADY_EXISTS, $"Book with name '{query.book.Name}' already exists!");

                return await Task.FromResult(response);
            }

            var bookToBeCreated = new BookModel();

            bookToBeCreated.Name = query.book.Name;
            bookToBeCreated.Author = query.book.Author;
            bookToBeCreated.ImgUri = query.book.ImgUri;
            bookToBeCreated.Genre = query.book.Genre;

            CommitCreateBook(bookToBeCreated);

            response.Data = bookToBeCreated;
        }
        catch (Exception ex)
        {
            response.PutError(ErrorCode.BOOK_COULD_NOT_BE_CREATED_EXCEPTION, $"{ex.Message} {ex.InnerException}");

            return await Task.FromResult(response);
        }

        return await Task.FromResult(response);
    }
}