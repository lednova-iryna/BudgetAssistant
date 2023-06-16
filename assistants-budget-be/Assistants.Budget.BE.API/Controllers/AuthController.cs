using Microsoft.AspNetCore.Mvc;
using Assistants.Budget.BE.Modules.Auth.CQRS;
using MediatR;

namespace Assistants.Budget.BE.API.Controllers;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> logger;
    private readonly IMediator mediator;

    public AuthController(ILogger<AuthController> logger, IMediator mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetClientCredentialsTokenAsync()
    {
        if (Request.Headers.TryGetValue("Authorization", out var basic))
        {
            var headerValue = basic.First();
            if (headerValue != null)
            {
                var creds = Base64Decode(headerValue.Split(" ").Last()).Split(":");
                return Ok(
                    await mediator.Send(
                        new ClientCredentialsTokenQuery { ClientId = creds.First(), ClientSecret = creds.Last() }
                    )
                );
            }
        }
        return BadRequest("Authorization header is required");
    }

    private static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}
