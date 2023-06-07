using Assistants.Budget.BE.Domain;
using FluentValidation;
using MediatR;

namespace Assistants.Budget.BE.BusinessLogic.Transactions.CQRS;

public class TransactionsCreateCommand : IRequest<Transaction>
{
    public DateTime Date { get; set; }
    public double Amount { get; set; }
    public string? Note { get; set; }
    public TransactionType Type { get; set; }

    public class Validator : AbstractValidator<TransactionsCreateCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Date).NotEqual(DateTime.MinValue).WithMessage("Should not be empty").NotNull();
            RuleFor(x => x.Amount).GreaterThan(0).NotNull();
            RuleFor(x => x.Type).NotNull();
        }
    }

    public class Handler : IRequestHandler<TransactionsCreateCommand, Transaction>
    {
        private readonly TransactionsService transactionsService;

        public Handler(TransactionsService transactionsService)
        {
            this.transactionsService = transactionsService;
        }

        public async Task<Transaction> Handle(TransactionsCreateCommand request, CancellationToken cancellationToken)
        {
            await new Validator().ValidateAndThrowAsync(request, cancellationToken);
            return await transactionsService.Create(request, cancellationToken);
        }
    }
}
