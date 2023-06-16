using Assistants.Budget.BE.Modules.Transactions.Domain;
using FluentValidation;

namespace Assistants.Budget.BE.Modules.Transactions.Validators;

class TransactionValidator : AbstractValidator<Transaction>
{
    public TransactionValidator()
    {
        RuleFor(x => x.CreatedAt).NotEmpty();
        RuleFor(x => x.CreatedBy).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Amount).NotEmpty();
        RuleFor(x => x.Type).NotNull();
    }
}
