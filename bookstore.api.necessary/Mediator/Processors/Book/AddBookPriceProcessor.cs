using bookstore.api.Mediator.Queries;
using bookstore.api.Models;
using bookstore.api.ResponseMessage;

namespace bookstore.api.necessary.Mediator.Processors.Book;

public interface AddBookPriceProcessor
{
    Task<ResponseMessage<BookModel>> AddBookPrice(CreateBookPriceQuery query);
}