using Assistants.Budget.BE.Modules.Core;
using Assistants.Budget.BE.Modules.Transactions.Domain;
using Assistants.Budget.BE.Modules.Transactions.Services;
using FluentValidation;
using MediatR;

namespace Assistants.Budget.BE.Modules.Transactions.CQRS;

public class TransactionsCreateCommand : IRequest<Transaction>
{
    public DateTime Date { get; set; }
    public double Amount { get; set; }
    public string? Note { get; set; }
    public TransactionType Type { get; set; }

    internal class Validator : AbstractValidator<TransactionsCreateCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Date).NotEqual(DateTime.MinValue).WithMessage("Should not be empty").NotNull();
            RuleFor(x => x.Amount).GreaterThan(0).NotEmpty();
            RuleFor(x => x.Type).NotNull();
        }
    }

    private class Handler : IRequestHandler<TransactionsCreateCommand, Transaction>
    {
        private readonly TransactionsService transactionsService;
        private readonly IRequestIdentityService requestIdentity;

        public Handler(TransactionsService transactionsService, IRequestIdentityService requestIdentity)
        {
            this.transactionsService = transactionsService;
            this.requestIdentity = requestIdentity;
        }

        public async Task<Transaction> Handle(TransactionsCreateCommand request, CancellationToken cancellationToken)
        {
            await new Validator().ValidateAndThrowAsync(request, cancellationToken);
            return await transactionsService.Create(
                request,
                requestIdentity.GetUserId().GetValueOrDefault(),
                cancellationToken
            );
        }
    }
}
