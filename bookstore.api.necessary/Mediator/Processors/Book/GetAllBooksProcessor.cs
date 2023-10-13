using bookstore.api.DTO;
using bookstore.api.Mediator.Queries;
using bookstore.api.Models;
using bookstore.api.necessary.DTO.Projections;
using bookstore.api.ResponseMessage;

namespace bookstore.api.necessary.Mediator.Processors.Book;

public interface GetAllBooksProcessor
{
    Task<ResponseMessage<List<BookProjection>>> GetAllBooks(GetAllBooksQuery query);
}