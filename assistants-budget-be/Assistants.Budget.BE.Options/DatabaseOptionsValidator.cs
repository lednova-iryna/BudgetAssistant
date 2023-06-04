using System;
using Assistants.Extensions.Options;
using FluentValidation;

namespace Assistants.Budget.BE.Options
{
    public class DatabaseOptionsValidator : AbstractValidator<DatabaseOptions>
    {
        public DatabaseOptionsValidator()
        {
            RuleFor(x => x.ConnectionString).NotNull().WithMessage($"{nameof(DatabaseOptions)}.{nameof(DatabaseOptions.ConnectionString)} is required.");
            RuleFor(x => x.Name).NotNull().WithMessage($"{nameof(DatabaseOptions)}.{nameof(DatabaseOptions.Name)} is required.");
        }
    }
}

