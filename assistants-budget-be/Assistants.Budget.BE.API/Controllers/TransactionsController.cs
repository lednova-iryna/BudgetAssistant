using MediatR;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
using Assistants.Budget.BE.Domain;
using Microsoft.Extensions.Options;
using Assistants.Budget.BE.Options;
using Assistants.Budget.BE.BusinessLogic.Transactions.CQRS;

namespace Assistants.Budget.BE.API.Controllers;

[ApiController]
[Route("transactions")]
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
    public async Task<IEnumerable<Transaction>> GetAsync([FromQuery] TransactionsQuery query)
    {
        return await mediator.Send(query);
    }


    [HttpPut]
    public async Task<IActionResult> CreateAsync([FromBody] TransactionsCreateCommand command)
    {
        var transaction = await mediator.Send(command);
        return Created($"transactions/{transaction.Id}", transaction);
    }
}
