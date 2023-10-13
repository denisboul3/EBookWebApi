using bookstore.api.DTO;
using bookstore.api.Mediator.Queries;
using bookstore.api.Models;
using bookstore.api.ResponseMessage;

namespace bookstore.api.necessary.Mediator.Processors.Book;

public interface DeleteBookProcessor
{
    Task<ResponseMessage<BookModel>> DeleteBook(DeleteBookQuery query);
}