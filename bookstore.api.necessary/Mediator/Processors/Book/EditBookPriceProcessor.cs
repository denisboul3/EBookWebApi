using bookstore.api.Mediator.Queries;
using bookstore.api.Models;
using bookstore.api.ResponseMessage;

namespace bookstore.api.necessary.Mediator.Processors.Book;

public interface EditBookPriceProcessor
{
    Task<ResponseMessage<PriceModel>> EditBookPrice(EditBookPriceQuery query);
}