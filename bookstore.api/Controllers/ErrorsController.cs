using AutoMapper;
using bookstore.api.DTO;
using bookstore.api.Extensions;
using bookstore.api.Mediator.Queries;
using bookstore.api.Models;
using bookstore.api.ResponseMessage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace E_Book_Store_API.Controllers;

[Produces("application/json")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/[controller]")]
[ApiController]
public class ErrorsController : HomeController
{
    private readonly IConfiguration _configuration;

    public ErrorsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [Route("get-errors", Name = "GetErrors")]
    [HttpGet]
    [Authorize(Roles = "SU")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Dictionary<int, string>))]
    public IActionResult GetErrors()
    {
        Dictionary<string, int> errorCodeDictionary = Enum.GetValues(typeof(ErrorCode))
            .Cast<ErrorCode>()
            .ToDictionary(errorCode => errorCode.ToString(), errorCode => (int)errorCode);

        return Ok(errorCodeDictionary);
    }
}