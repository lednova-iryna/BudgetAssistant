using MediatR;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
using Assistants.Budget.BE.Domain;
using Assistants.Budget.BE.Mediator.Transactions;

namespace Assistants.Budget.BE.API.Controllers;

[ApiController]
[Route("home")]
public class HomeController : ControllerBase
{
    private readonly ILogger<HomeController> logger;
    private readonly IMediator mediator;

    public HomeController(ILogger<HomeController> logger, IMediator mediator)
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
