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

namespace bookstore.api.necessary.Mediator.Handlers.User;

public class AddBookPriceHandler : AddBookPriceProcessor, IRequestHandler<CreateBookPriceQuery, ResponseMessage<BookModel>>
{
    private readonly IUoW _uOf;
    private readonly IBookRepository _bookRepository;

    public AddBookPriceHandler(IUoW uOf, IBookRepository bookRepository)
    {
        _uOf = uOf;
        _bookRepository = bookRepository;
    }

    public async Task<ResponseMessage<BookModel>> Handle(CreateBookPriceQuery request, CancellationToken cancellationToken)
    {
        return await AddBookPrice(request);
    }

    private void CommitBook(BookModel bookToBeUpdated)
    {
        _bookRepository.Save(bookToBeUpdated);
        _uOf.Commit();
    }

    public async Task<ResponseMessage<BookModel>> AddBookPrice(CreateBookPriceQuery query)
    {
        var response = new ResponseMessage<BookModel>();
        try
        {
            var bookToBeUpdated = _bookRepository.FindBy(query.bookPrice.BookId);

            if (bookToBeUpdated.IsNull())
            {
                response.PutError(ErrorCode.BOOK_NOT_FOUND, $"Could not find book with ID {query.bookPrice.BookId}");

                return await Task.FromResult(response);
            }

            if (query.bookPrice.ForDay <= 0 || query.bookPrice.ForDay > 7)
            {
                response.PutError(ErrorCode.PRICE_DAY_NOT_VALID, $"Field 'ForDay' accepts only 1 - 7");

                return await Task.FromResult(response);
            }

            var bookPrice = new PriceModel();
            bookPrice.Price = query.bookPrice.Price;
            bookPrice.Book = bookToBeUpdated;
            bookPrice.ForDay = query.bookPrice.ForAllDays ? 0 : query.bookPrice.ForDay;
            bookPrice.ForAllDays = query.bookPrice.ForAllDays;
            bookToBeUpdated.Prices.Add(bookPrice);

            CommitBook(bookToBeUpdated);

            response.Data = bookToBeUpdated;
        }
        catch (Exception ex)
        {
            response.PutError(ErrorCode.BOOK_NOT_FOUND_EXCEPTION, $"{ex.Message} {ex.InnerException}");

            return await Task.FromResult(response);
        }

        return await Task.FromResult(response);
    }
}