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
using static System.Reflection.Metadata.BlobBuilder;
using bookstore.api.necessary.DTO.Projections;

namespace bookstore.api.necessary.Mediator.Handlers.User;

public class GetAllBooksHandler : GetAllBooksProcessor, IRequestHandler<GetAllBooksQuery, ResponseMessage<List<BookProjection>>>
{
    private readonly IMapper _mapper;
    private readonly IBookRepository _bookRepository;

    public GetAllBooksHandler(IMapper mapper, IBookRepository bookRepository)
    {
        _mapper = mapper;
        _bookRepository = bookRepository;
    }

    public async Task<ResponseMessage<List<BookProjection>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        return await GetAllBooks(request);
    }

    public async Task<ResponseMessage<List<BookProjection>>> GetAllBooks(GetAllBooksQuery query)
    {
        var response = new ResponseMessage<List<BookProjection>>();

        try
        {
            var book = _bookRepository.GetAll();

            if(book.IsNull())
            {
                response.PutError(ErrorCode.BROWSE_BOOKS_NOT_FOUND, $"Couldn't find any books at database");

                return await Task.FromResult(response);
            }

            response.Data = _mapper.Map<List<BookProjection>>(book);
        }
        catch (Exception ex)
        {
            response.PutError(ErrorCode.BROWSE_BOOKS_EXCEPTION, $"{ex.Message} {ex.InnerException}");

            return await Task.FromResult(response);
        }

        return await Task.FromResult(response);
    }
}