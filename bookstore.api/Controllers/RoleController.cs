using AutoMapper;
using bookstore.api.DTO;
using bookstore.api.Mediator.Queries;
using bookstore.api.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace E_Book_Store_API.Controllers;

[Produces("application/json")]
[ResponseCache(Duration = 0, NoStore = true, VaryByHeader = "*")]
[Route("api/[controller]")]
[ApiController]
public class RoleController : HomeController
{
    private readonly IMediator _mediator;
    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Route("create-role", Name = "CreateRole")]
    [HttpPost]
    [Authorize(Roles = "SU")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(RoleModel))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Dictionary<string, object>))]
    public async Task<ActionResult> CreateRole([FromBody] CreateRoleDto role)
    {
        var createdRole = await _mediator.Send(new CreateRoleCommand(Guid.Empty, role.Name));
        
        if (!createdRole.NoErrors())
        {
            return BadRequest(createdRole.Result);
        }

        return Ok(createdRole.Data);
    }

    [Route("assign-role", Name = "AssignRole")]
    [HttpPost]
    [Authorize(Roles = "SU, Admin")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserModel))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(Dictionary<string, object>))]
    public async Task<ActionResult> AssignRole([FromBody] AssignRoleDto role)
    {
        var modifiedRole = await _mediator.Send(new AssignRoleCommand(GetUserID(), role.RoleId, role.AssignedTo));
        
        if (!modifiedRole.NoErrors())
        {
            return BadRequest(modifiedRole.Result);
        }

        return Ok(modifiedRole.Data);
    }
}