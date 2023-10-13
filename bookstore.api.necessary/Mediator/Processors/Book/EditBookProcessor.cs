using bookstore.api.Mediator.Queries;
using bookstore.api.Models;
using bookstore.api.ResponseMessage;

namespace bookstore.api.necessary.Mediator.Processors.Book;

public interface EditBookProcessor
{
    Task<ResponseMessage<BookModel>> EditBook(EditBookQuery query);
}