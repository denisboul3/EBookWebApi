using AutoMapper;
using bookstore.api.DTO;
using bookstore.api.Mediator.Queries;
using bookstore.api.Models;
using bookstore.api.necessary.DTO.Projections;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Diagnostics.Metrics;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace E_Book_Store_API.Controllers;

[Produces("application/json")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/[controller]")]
[ApiController]
public class BookController : HomeController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public BookController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [Route("add-book", Name = "AddBook")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BookProjection))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Dictionary<string, object>))]
    public async Task<ActionResult> AddBook([FromBody] CreateBookDto book)
    {
        var createdBook = await _mediator.Send(new CreateBookQuery(book));
        if (!createdBook.NoErrors())
        {
            return BadRequest(createdBook.Result);
        }

        var res = _mapper.Map<BookProjection>(createdBook.Data);

        return Ok(res);
    }

    [Route("add-book-price", Name = "AddBookPrice")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BookProjection))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Dictionary<string, object>))]
    public async Task<ActionResult> AddBookPrice([FromBody] CreateBookPriceDto book)
    {
        var createdBook = await _mediator.Send(new CreateBookPriceQuery(book));

        if (!createdBook.NoErrors())
        {
            return BadRequest(createdBook.Result);
        }

        var res = _mapper.Map<BookProjection>(createdBook.Data);

        return Ok(res);
    }

    [Route("edit-book", Name = "EditBook")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BookModel))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Dictionary<string, object>))]
    public async Task<ActionResult> EditBook([FromBody] EditBookDto book)
    {
        var modifiedBook = await _mediator.Send(new EditBookQuery(book));

        if (!modifiedBook.NoErrors())
        {
            return BadRequest(modifiedBook.Result);
        }

        var res = _mapper.Map<BookProjection>(modifiedBook.Data);

        return Ok(res);
    }

    [Route("edit-book-price", Name = "EditBookPrice")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PriceProjection))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Dictionary<string, object>))]
    public async Task<ActionResult> EditBookPrice([FromBody] EditBookPriceDto book)
    {
        var createdBook = await _mediator.Send(new EditBookPriceQuery(book));

        if (!createdBook.NoErrors())
        {
            return BadRequest(createdBook.Result);
        }

        var res = _mapper.Map<PriceProjection>(createdBook.Data);

        return Ok(res);
    }

    [Route("delete-book", Name = "DeleteBook")]
    [HttpDelete]
    [Authorize(Roles = "SU")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BookModel))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Dictionary<string, object>))]
    public async Task<ActionResult> DeleteBook(Guid id)
    {
        var deletedBook = await _mediator.Send(new DeleteBookQuery(id));

        return Ok(deletedBook);
    }

    [Route("browse-books", Name = "BrowseBooks")]
    [HttpGet]
    [Authorize(Roles = "SU, Admin, User")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<BookProjection>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Dictionary<string, object>))]
    public async Task<ActionResult> BrowseBooks()
    {
        var books = await _mediator.Send(new GetAllBooksQuery());
        
        if (!books.NoErrors())
        {
            return BadRequest(books.Result);
        }

        return Ok(books.Data);
    }

    [Route("book-information", Name = "BookInfo")]
    [HttpGet]
    [Authorize(Roles = "SU, Admin, User")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BookProjection))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Dictionary<string, object>))]
    public async Task<ActionResult> BookInfo(Guid bookId)
    {
        var bookInfo = await _mediator.Send(new GetBookInfoQuery(bookId));

        if (!bookInfo.NoErrors())
        {
            return BadRequest(bookInfo.Result);
        }

        return Ok(bookInfo.Data);
    }
}