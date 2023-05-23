using MediatR;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
using Assistants.Budget.BE.Domain;
using Assistants.Budget.BE.Mediator.Transactions;
using Microsoft.Extensions.Options;
using Assistants.Budget.BE.Options;

namespace Assistants.Budget.BE.API.Controllers;

[ApiController]
[Route("home")]
public class HomeController : ControllerBase
{
    private readonly ILogger<HomeController> logger;
    private readonly IMediator mediator;
    private readonly DatabaseOptions databaseOptions;

    public HomeController(ILogger<HomeController> logger, IMediator mediator, IOptions<DatabaseOptions> databaseOptions)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.databaseOptions = databaseOptions.Value;
    }

    [HttpGet]
    public  string Get()
    {
        return databaseOptions.ConnectionString; //await mediator.Send(new TransactionsQuery());
    }
}
