using Assistants.Budget.BE.Modules.Core;
using Assistants.Budget.BE.Modules.Transactions.Services;
using FluentValidation;
using MediatR;

namespace Assistants.Budget.BE.Modules.Transactions.CQRS;

public class TransactionsDeleteCommand : IRequest
{
    public Guid? TransactionId { get; set; }

    IValidator<TransactionsDeleteCommand> validator { get; } = new Validator();

    class Validator : AbstractValidator<TransactionsDeleteCommand>
    {
        public Validator()
        {
            RuleFor(x => x.TransactionId).NotEmpty().NotEqual(Guid.Empty);
        }
    }

    class Handler : IRequestHandler<TransactionsDeleteCommand>
    {
        private readonly TransactionsService transactionsService;

        public Handler(TransactionsService transactionsService)
        {
            this.transactionsService = transactionsService;
        }

        public async Task Handle(TransactionsDeleteCommand request, CancellationToken cancellationToken)
        {
            await request.validator.ValidateAndThrowAsync(request, cancellationToken);

            await transactionsService.DeleteAsync(
               request.TransactionId!.Value,
               cancellationToken
           );
        }
    }
}

