using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Assistants.Budget.BE.Modules.Core;

namespace Assistants.Budget.BE.API.Services;

public class RequestIdentityService : IRequestIdentityService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public RequestIdentityService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetUserId()
    {
        var userIdentity = (ClaimsIdentity?)httpContextAccessor?.HttpContext?.User.Identity;
        if (Guid.TryParse(userIdentity?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value, out var userId))
        {
            return userId;
        }
        return null;
    }
}
