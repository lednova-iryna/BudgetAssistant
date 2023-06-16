using Assistants.Budget.BE.Modules.Transactions.Domain;
using Assistants.Budget.BE.Modules.Transactions.Services;
using FluentValidation;
using MediatR;

namespace Assistants.Budget.BE.Modules.Transactions.CQRS;

public class TransactionsQueryOne : IRequest<Transaction>
{
    public Guid Id { get; set; }

    private class Validator : AbstractValidator<TransactionsQueryOne>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }

    private class Handler : IRequestHandler<TransactionsQueryOne, Transaction>
    {
        private readonly TransactionsService transactionsService;

        public Handler(TransactionsService transactionsService)
        {
            this.transactionsService = transactionsService;
        }

        public async Task<Transaction> Handle(TransactionsQueryOne request, CancellationToken cancellationToken)
        {
            await new Validator().ValidateAndThrowAsync(request, cancellationToken);
            return await transactionsService.GetById(request.Id, cancellationToken);
        }
    }
}
