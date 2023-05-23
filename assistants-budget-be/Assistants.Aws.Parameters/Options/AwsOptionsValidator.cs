using FluentValidation;

namespace Assistants.Aws.Parameters.Options;

public class AwsOptionsValidator : AbstractValidator<AwsOptions>
{
    public AwsOptionsValidator()
    {
        var sectionName = new AwsOptions().SectionName;
    }
}

