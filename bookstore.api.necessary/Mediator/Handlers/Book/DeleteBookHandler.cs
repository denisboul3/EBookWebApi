using AutoMapper;
using ISession = NHibernate.ISession;
using UnitOfWork;
using bookstore.api.Models;
using MediatR;
using bookstore.api.Mediator.Queries;
using bookstore.api.ResponseMessage;
using bookstore.api.necessary.Mediator.Processors.Book;
using bookstore.api.necessary.Repositories.IRepositories.Book;
using bookstore.api.DTO;
using bookstore.api.Extensions;

namespace bookstore.api.necessary.Mediator.Handlers.User;

public class DeleteBookHandler : DeleteBookProcessor, IRequestHandler<DeleteBookQuery, ResponseMessage<BookModel>>
{
    private readonly IUoW _uOf;
    private readonly IBookRepository _bookRepository;

    public DeleteBookHandler(IUoW uOf, IBookRepository bookRepository)
    {
        _uOf = uOf;
        _bookRepository = bookRepository;
    }

    public async Task<ResponseMessage<BookModel>> Handle(DeleteBookQuery request, CancellationToken cancellationToken)
    {
        return await DeleteBook(request);
    }

    private void CommitDeletion(BookModel book)
    {
        _bookRepository.Remove(book);
        _uOf.Commit();
    }

    public async Task<ResponseMessage<BookModel>> DeleteBook(DeleteBookQuery query)
    {
        var response = new ResponseMessage<BookModel>();
        try
        {
            var book = _bookRepository.FindBy(query.bookId);

            if(book.IsNull())
            {
                response.PutError(ErrorCode.BOOK_NOT_FOUND, $"Couldn't find book with ID '{query.bookId}'");
                return await Task.FromResult(response);
            }

            CommitDeletion(book);
        }
        catch (Exception ex)
        {
            response.PutError(ErrorCode.BOOK_DELETE_EXCEPTION, $"{ex.Message} {ex.InnerException}");
            return await Task.FromResult(response);
        }

        return await Task.FromResult(response);
    }
}