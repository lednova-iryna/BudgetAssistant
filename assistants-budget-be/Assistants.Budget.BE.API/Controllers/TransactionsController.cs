using MediatR;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
using Assistants.Budget.BE.Domain;
using Microsoft.Extensions.Options;
using Assistants.Budget.BE.Options;
using Assistants.Budget.BE.BusinessLogic.Transactions.CQRS;
using Assistants.Budget.BE.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace Assistants.Budget.BE.API.Controllers;

[ApiController]
[Route("transactions")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ILogger<TransactionsController> logger;
    private readonly IMediator mediator;
    private readonly DatabaseOptions databaseOptions;

    public TransactionsController(ILogger<TransactionsController> logger, IMediator mediator, IOptions<DatabaseOptions> databaseOptions)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.databaseOptions = databaseOptions.Value;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Transaction>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<ValidationErrorResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<Transaction>> GetAsync([FromQuery] TransactionsQuery query)
    {
        return await mediator.Send(query);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Transaction), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<ValidationErrorResponse>), StatusCodes.Status400BadRequest)]
    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        TransactionsQueryOne query = new()
        {
            Id = id
        };
        return await mediator.Send(query);
    }

    [HttpPut]
    [ProducesResponseType(typeof(Transaction), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(IEnumerable<ValidationErrorResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] TransactionsCreateCommand command)
    {
        var transaction = await mediator.Send(command);
        return Created($"transactions/{transaction.Id}", transaction);
    }
}
