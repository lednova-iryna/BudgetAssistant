using Assistants.Extensions.Options;

namespace Assistants.Libs.Aws.Parameters.Options;

class AwsParametersOptions : BaseOptions
{
    public override string SectionName => "Parameters";

    public bool Ignore { get; set; } = true;
    public string? Names { get; set; }

    /// <summary>
    /// Seconds
    /// </summary>
    public int SecretPollingInterval { get; set; } = 60 * 60 * 24;
}
