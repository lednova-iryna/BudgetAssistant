using System;
using Assistants.Budget.BE.Mediator.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace Assistants.Budget.BE.Mediator;

public static class AddMediatorConfiguration
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        return services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<TransactionsQuery>();
        });
    }
}

