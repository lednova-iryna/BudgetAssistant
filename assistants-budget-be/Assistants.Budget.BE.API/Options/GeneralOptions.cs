using Assistants.Extensions.Options;
using FluentValidation;

namespace Assistants.Budget.BE.Options;

public class GeneralOptions : BaseOptions
{
    public override string SectionName => "General";

    public string Environment { get; set; }
    public bool IsSwaggerEnabled { get; set; } = false;

    public class Validator : AbstractValidator<GeneralOptions>
    {
        public Validator()
        {
            RuleFor(x => x.Environment).NotEmpty();
        }
    }
}
