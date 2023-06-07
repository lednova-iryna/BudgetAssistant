using Microsoft.Extensions.DependencyInjection;
using Assistants.Budget.BE.BusinessLogic.Transactions;
using Assistants.Budget.BE.BusinessLogic.Transactions.CQRS;
using FluentValidation;

namespace Assistants.Budget.BE.BusinessLogic;

public static class Configurator
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
