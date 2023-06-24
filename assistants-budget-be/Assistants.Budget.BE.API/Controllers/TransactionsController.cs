using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Assistants.Budget.BE.API.Models;
using Assistants.Budget.BE.Modules.Transactions.CQRS;
using Assistants.Budget.BE.Modules.Transactions.Domain;
using MediatR;

namespace Assistants.Budget.BE.API.Controllers;

[Authorize]
[ApiController]
[Route("transactions")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator mediator;

    public TransactionsController(IMediator mediator)
    {
        this.mediator = mediator;
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
        TransactionsQueryOne query = new() { Id = id };
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

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Transaction), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(IEnumerable<ValidationErrorResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await mediator.Send(new TransactionsDeleteCommand { TransactionId = id });
        return NoContent();
    }
}
