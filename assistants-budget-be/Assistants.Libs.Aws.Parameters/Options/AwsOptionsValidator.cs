using FluentValidation;

namespace Assistants.Libs.Aws.Parameters.Options;

class AwsOptionsValidator : AbstractValidator<AwsOptions>
{
    public AwsOptionsValidator()
    {
        var sectionName = new AwsOptions().SectionName;
    }
}
