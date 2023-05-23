using Assistants.Extensions.Options;
namespace Assistants.Aws.Parameters.Options;

public class AwsOptions : BaseOptions
{
    public override string SectionName => "Aws";

    public AwsParametersOptions? Parameters { get; set; }
}

