using MediatR;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
using Assistants.Budget.BE.Domain;
using Assistants.Budget.BE.Mediator.Transactions;

namespace Assistants.Budget.BE.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ILogger<TransactionController> logger;
    private readonly IMediator mediator;

    public TransactionController(ILogger<TransactionController> logger, IMediator mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<Transaction>> Get()
    {
        return await mediator.Send(new TransactionsQuery());
    }
}

