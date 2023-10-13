using AutoMapper;
using bookstore.api.DTO;
using bookstore.api.Extensions;
using bookstore.api.Mediator.Queries;
using bookstore.api.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Nethereum.Signer;
using Solnet.KeyStore.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;

namespace E_Book_Store_API.Controllers;

[Produces("application/json")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/[controller]")]
[ApiController]
public class UserController : HomeController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    private readonly IConfiguration _configuration;

    public UserController(IMediator mediator, IMapper mapper, IConfiguration configuration)
    {
        _mediator = mediator;
        _mapper = mapper;
        _configuration = configuration;
    }

    [Route("create-user", Name = "CreateUser")]
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserModel))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Dictionary<string, object>))]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserDto user)
    {
        var createdUser = await _mediator.Send(new CreateUserQuery(user.Login, user.Password.ToSha3(), user.Email));

        if (!createdUser.NoErrors())
        {
            return BadRequest(createdUser.Result);
        }

        return Ok(createdUser.Data);

    }

    [Route("get-users", Name = "GetUsers")]
    [HttpGet]
    [Authorize(Roles = "SU")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IList<UserModel>))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Dictionary<string, object>))]
    public async Task<ActionResult> GetUsers()
    {
        var products = await _mediator.Send(new GetUsersQuery());

        return Ok(products.Data);
    }

    [Route("login", Name = "UserLogin")]
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AuthDto))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Dictionary<string, object>))]
    public async Task<ActionResult> UserLogin([FromBody] LoginDto loginData)
    {
        try
        {
            var fetchedUser = await _mediator.Send(new GetUserByLoginPasswordQuery(loginData.Login, loginData.Password.ToSha3()));
            if(!fetchedUser.NoErrors())
            {
                return BadRequest(fetchedUser.Result);
            }

            var tokenValue = CreateAccessToken(fetchedUser.Data, TimeSpan.FromMinutes(36000));

            return Ok(new AuthDto{
                Token = tokenValue
            });
        }
        catch (Exception e)
        {
            return BadRequest($"ERROR_FETCHING_USER {e.Message}");
        }
    }
    private string CreateAccessToken(UserModel user, TimeSpan expiration)
    {
        var keyBytes = Encoding.UTF8.GetBytes(_configuration.GetSection("AuthOptions:SigningKey").Value);
        var symmetricKey = new SymmetricSecurityKey(keyBytes);

        var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Sid, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login)
        };

        if (!user.Role.IsNull())
        {
            claims.Add(new Claim(ClaimTypes.Role, user.Role.Name));
        }

        var token = new JwtSecurityToken(
            issuer: _configuration.GetSection("AuthOptions:Issuer").Value,
            audience: _configuration.GetSection("AuthOptions:Audience").Value,
            claims: claims,
            expires: DateTime.Now.Add(expiration),
            signingCredentials: signingCredentials);

        var rawToken = new JwtSecurityTokenHandler().WriteToken(token);
        return rawToken;
    }


    [Route("buy", Name = "BuyBook")]
    [HttpPost]
    [AllowAnonymous]
    public ActionResult BuyBook()
    {
        var x = new UserDto();
        return Ok(x);
    }

    [Route("change-password", Name = "ChangePassword")]
    [HttpPost]
    [Authorize]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserModel))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Dictionary<string, object>))]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto query)
    {
        try
        {
            var modifiedUser = await _mediator.Send(new ChangeUserPasswordQuery(GetUserLogin(), query.OldPassword.ToSha3(), query.NewPassword.ToSha3()));
            if (!modifiedUser.NoErrors())
            {
                return BadRequest(modifiedUser.Result);
            }

            return Ok(modifiedUser);
        }
        catch (Exception e)
        {
            return BadRequest($"ERROR_CHANGING_PASSWORD {e.Message}");
        }
    }
}