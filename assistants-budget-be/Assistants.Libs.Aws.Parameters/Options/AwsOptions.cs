using Assistants.Extensions.Options;

namespace Assistants.Libs.Aws.Parameters.Options;

class AwsOptions : BaseOptions
{
    public override string SectionName => "Aws";

    public AwsParametersOptions? Parameters { get; set; }
}
