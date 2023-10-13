using bookstore.api.ResponseMessage;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Book_Store_API.Controllers;

public abstract class HomeController : ControllerBase
{
    protected Guid GetUserID()
    {
        var claimsPrincipal = User;
        var guid = claimsPrincipal?.FindFirst(ClaimTypes.Sid)?.Value;
        return Guid.TryParse(guid, out _) ? Guid.Parse(guid) : Guid.Empty;
    }

    protected string GetUserLogin()
    {
        var claimsPrincipal = User;
        var loginName = claimsPrincipal?.FindFirst(ClaimTypes.Name)?.Value;
        return loginName ?? "";
    }

    protected string GetLoggedEmail()
    {
        var claimsPrincipal = User;
        var email = claimsPrincipal?.FindFirst(ClaimTypes.Email)?.Value;
        return email ?? "";
    }
}
