using AutoMapper;
using ISession = NHibernate.ISession;
using UnitOfWork;
using bookstore.api.Models;
using MediatR;
using bookstore.api.Mediator.Queries;
using bookstore.api.ResponseMessage;
using bookstore.api.necessary.Mediator.Processors.Book;
using bookstore.api.necessary.Repositories.IRepositories.Book;
using bookstore.api.Extensions;
using bookstore.api.necessary.Repositories.IRepositories.Price;

namespace bookstore.api.necessary.Mediator.Handlers.User;

public class EditBookHandler : EditBookProcessor, IRequestHandler<EditBookQuery, ResponseMessage<BookModel>>
{
    private readonly IUoW _uOf;
    private readonly IBookRepository _bookRepository;

    public EditBookHandler(IUoW uOf, IBookRepository bookRepository)
    {
        _uOf = uOf;
        _bookRepository = bookRepository;
    }

    public async Task<ResponseMessage<BookModel>> Handle(EditBookQuery request, CancellationToken cancellationToken)
    {
        return await EditBook(request);
    }

    private void CommitBookEditing(BookModel editedBook)
    {
        _bookRepository.Save(editedBook);
        _uOf.Commit();
    }

    public async Task<ResponseMessage<BookModel>> EditBook(EditBookQuery query)
    {
        var response = new ResponseMessage<BookModel>();
        try
        {
            var bookToBeUpdated = _bookRepository.FindBy(query.book.Id);
            var bookName = _bookRepository.FindBy(query.book.Name, "Name");

            if (!bookName.IsNull())
            {
                response.PutError(ErrorCode.BOOK_NAME_ALREADY_EXISTS, $"Book name '{query.book.Name}' already exists");

                return await Task.FromResult(response);
            }

            if (bookToBeUpdated.IsNull())
            {
                response.PutError(ErrorCode.BOOK_NOT_FOUND, $"Could not find book with ID {query.book.Id}");

                return await Task.FromResult(response);
            }

            bookToBeUpdated.Author = query.book.Author;
            bookToBeUpdated.Name = query.book.Name;
            bookToBeUpdated.Genre = query.book.Genre;
            bookToBeUpdated.ImgUri = query.book.ImgUri;

            CommitBookEditing(bookToBeUpdated);

            response.Data = bookToBeUpdated;
        }
        catch (Exception ex)
        {
            response.PutError(ErrorCode.BOOK_MODIFY_EXCEPTION, $"{ex.Message} {ex.InnerException}");

            return await Task.FromResult(response);
        }

        return await Task.FromResult(response);
    }
}