using Assistants.Budget.BE.Modules.Transactions.Domain;
using Assistants.Budget.BE.Modules.Transactions.Services;
using FluentValidation;
using MediatR;

namespace Assistants.Budget.BE.Modules.Transactions.CQRS;

public class TransactionsQuery : IRequest<IEnumerable<Transaction>>
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public TransactionType? Type { get; set; }

    private class Validator : AbstractValidator<TransactionsQuery>
    {
        public Validator()
        {
            RuleFor(x => x.FromDate).NotEmpty().NotNull();
            RuleFor(x => x.ToDate).NotEmpty().NotNull();
            RuleFor(x => x)
                .Must(x => x.ToDate > x.FromDate)
                .OverridePropertyName("ToDate")
                .WithMessage("ToDate must be great than FromDate");
        }
    }

    private class Handler : IRequestHandler<TransactionsQuery, IEnumerable<Transaction>>
    {
        private readonly TransactionsService transactionsService;

        public Handler(TransactionsService transactionsService)
        {
            this.transactionsService = transactionsService;
        }

        public async Task<IEnumerable<Transaction>> Handle(
            TransactionsQuery request,
            CancellationToken cancellationToken
        )
        {
            new Validator().ValidateAndThrow(request);
            return await transactionsService.Get(request, cancellationToken);
        }
    }
}
