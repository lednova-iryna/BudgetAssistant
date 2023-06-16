using Microsoft.Extensions.DependencyInjection;
using Assistants.Budget.BE.Modules.Transactions.CQRS;
using Assistants.Budget.BE.Modules.Transactions.Services;
using FluentValidation;

namespace Assistants.Budget.BE.Modules.Transactions;

public static class Configurator
{
    public static IServiceCollection AddTransactionsModule(this IServiceCollection services)
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
