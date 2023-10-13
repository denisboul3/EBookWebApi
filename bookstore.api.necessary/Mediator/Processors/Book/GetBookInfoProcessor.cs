using bookstore.api.DTO;
using bookstore.api.Mediator.Queries;
using bookstore.api.Models;
using bookstore.api.necessary.DTO.Projections;
using bookstore.api.ResponseMessage;

namespace bookstore.api.necessary.Mediator.Processors.Book;

public interface GetBookInfoProcessor
{
    Task<ResponseMessage<BookProjection>> GetBookInfo(GetBookInfoQuery query);
}