using bookstore.api.DTO;
using bookstore.api.Models;
using bookstore.api.necessary.DTO.Projections;
using bookstore.api.ResponseMessage;
using MediatR;

namespace bookstore.api.Mediator.Queries;

public record EditBookQuery(EditBookDto book) : IRequest<ResponseMessage<BookModel>>;
public record EditBookPriceQuery(EditBookPriceDto price) : IRequest<ResponseMessage<PriceModel>>;
public record CreateBookQuery(CreateBookDto book) : IRequest<ResponseMessage<BookModel>>;
public record CreateBookPriceQuery(CreateBookPriceDto bookPrice) : IRequest<ResponseMessage<BookModel>>;
public record GetBookInfoQuery(Guid bookId) : IRequest<ResponseMessage<BookProjection>>;
public record GetAllBooksQuery() : IRequest<ResponseMessage<List<BookProjection>>>;
public record DeleteBookQuery(Guid bookId) : IRequest<ResponseMessage<BookModel>>;