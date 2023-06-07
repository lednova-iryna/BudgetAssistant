using Assistants.Budget.BE.Domain;
using FluentValidation;
using MediatR;

namespace Assistants.Budget.BE.BusinessLogic.Transactions.CQRS;

public class TransactionsQueryOne : IRequest<Transaction>
{
    public Guid Id { get; set; }

    public class Validator : AbstractValidator<TransactionsQueryOne>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }

    public class Handler : IRequestHandler<TransactionsQueryOne, Transaction>
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
