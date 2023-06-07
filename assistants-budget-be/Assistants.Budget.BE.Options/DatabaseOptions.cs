using Assistants.Extensions.Options;
using FluentValidation;

namespace Assistants.Budget.BE.Options;

public class DatabaseOptions : BaseOptions
{
    public override string SectionName => "Database";

    public string ConnectionString { get; set; }
    public string Name { get; set; }

    public class Validator : AbstractValidator<DatabaseOptions>
    {
        public Validator()
        {
            RuleFor(x => x.ConnectionString)
                .NotNull()
                .WithMessage($"{nameof(DatabaseOptions)}.{nameof(DatabaseOptions.ConnectionString)} is required.");
            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage($"{nameof(DatabaseOptions)}.{nameof(DatabaseOptions.Name)} is required.");
        }
    }
}
