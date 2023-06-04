using System;
using Assistants.Budget.BE.BusinessLogic.Transactions;
using Assistants.Budget.BE.BusinessLogic.Transactions.CQRS;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Assistants.Budget.BE.BusinessLogic;

public static class AddMediatorConfiguration
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        return services
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<TransactionsQuery>();
            })
            .AddScoped<TransactionsService>()
            .AddValidatorsFromAssemblyContaining<TransactionsCreateCommand.Validator>();
    }
}

