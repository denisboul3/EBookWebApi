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
using bookstore.api.necessary.DTO.Projections;

namespace bookstore.api.necessary.Mediator.Handlers.User;

public class GetBookInfoHandler : GetBookInfoProcessor, IRequestHandler<GetBookInfoQuery, ResponseMessage<BookProjection>>
{
    private readonly IMapper _mapper;
    private readonly IBookRepository _bookRepository;

    public GetBookInfoHandler(IMapper mapper, IBookRepository bookRepository)
    {
        _mapper = mapper;
        _bookRepository = bookRepository;
    }

    public async Task<ResponseMessage<BookProjection>> Handle(GetBookInfoQuery request, CancellationToken cancellationToken)
    {
        return await GetBookInfo(request);
    }

    public async Task<ResponseMessage<BookProjection>> GetBookInfo(GetBookInfoQuery query)
    {
        var response = new ResponseMessage<BookProjection>();

        try
        {
            var book = _bookRepository.FindBy(query.bookId);

            if(book.IsNull())
            {
                response.PutError(ErrorCode.BOOK_NOT_FOUND, $"Couldn't find book with ID '{query.bookId}'");

                return await Task.FromResult(response);
            }

            response.Data = _mapper.Map<BookProjection>(book);
        }
        catch (Exception ex)
        {
            response.PutError(ErrorCode.BOOK_NOT_FOUND_EXCEPTION, $"{ex.Message} {ex.InnerException}");

            return await Task.FromResult(response);
        }

        return await Task.FromResult(response);
    }
}