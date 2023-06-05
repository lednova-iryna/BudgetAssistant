using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assistants.Budget.BE.API.Models;
using Assistants.Budget.BE.BusinessLogic.Auth.CQRS;
using Assistants.Budget.BE.BusinessLogic.Transactions.CQRS;
using Assistants.Budget.BE.Options;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var creds = Base64Decode(basic.First().Split(" ").Last()).Split(":");
            return Ok(await mediator.Send(new ClientCredentialsTokenQuery
            {
                ClientId = creds.First(),
                ClientSecret = creds.Last()
            }));
        }
        return BadRequest("Authorization header is required");
    }

    private static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}
