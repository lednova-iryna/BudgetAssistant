using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Assistants.Budget.BE.API.Models;
using Assistants.Budget.BE.Modules.Auth.CQRS;
using Assistants.Budget.BE.Modules.Auth.Domain;
using Assistants.Budget.BE.Modules.Auth.Models;
using Assistants.Libs.AspNetCore.Auth;
using MediatR;

namespace Assistants.Budget.BE.API.Controllers;

[Authorize]
[ApiController]
[Route("identity")]
public class IdentityController : ControllerBase
{
    private readonly ILogger<IdentityController> logger;
    private readonly IMediator mediator;

    public IdentityController(ILogger<IdentityController> logger, IMediator mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    [HttpGet("roles")]
    [Permission(IdentityPermissions.RoleCanRead)]
    [ProducesResponseType(typeof(IEnumerable<IdentityRole>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<ValidationErrorResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<IdentityRole>> GetRolesAsync([FromQuery] IdentityRoleQuery query)
    {
        return await mediator.Send(query);
    }

    [HttpGet("roles/{id}")]
    [Permission(IdentityPermissions.RoleCanRead)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(IdentityRole), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<ValidationErrorResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IdentityRole?> GetRoleByIdAsync(string id)
    {
        IdentityRoleQueryOne query = new() { Id = id };
        return await mediator.Send(query);
    }

    [HttpPut("roles")]
    [Permission(IdentityPermissions.RoleCanCreate)]
    [ProducesResponseType(typeof(IdentityRole), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(IEnumerable<ValidationErrorResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRoleAsync([FromBody] IdentityRoleCreateCommand command)
    {
        var identityRole = await mediator.Send(command);
        return Created($"roles/{identityRole.Id}", identityRole);
    }

    [HttpGet("users")]
    [Permission(IdentityPermissions.UserCanRead)]
    [ProducesResponseType(typeof(IEnumerable<IdentityUser>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<ValidationErrorResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<IdentityUser>> GetUsersAsync([FromQuery] IdentityUserQuery query)
    {
        return await mediator.Send(query);
    }

    [HttpGet("users/{id}")]
    [Permission(IdentityPermissions.UserCanRead)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(IdentityRole), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<ValidationErrorResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IdentityUser?> GetUserByIdAsync(string id)
    {
        IdentityUserQueryOne query = new() { Id = id };
        return await mediator.Send(query);
    }

    [HttpPut("users")]
    [Permission(IdentityPermissions.UserCanCreate)]
    [ProducesResponseType(typeof(IdentityUser), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(IEnumerable<ValidationErrorResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserAsync([FromBody] IdentityUserCreateCommand command)
    {
        var user = await mediator.Send(command);
        return Created($"users/{user.Id}", user);
    }
}
