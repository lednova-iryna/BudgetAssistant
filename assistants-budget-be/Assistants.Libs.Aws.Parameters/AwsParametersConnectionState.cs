using Assistants.Libs.Aws.Parameters.Constants;

namespace Assistants.Libs.Aws.Parameters;

public static class AwsParametersConnectionState
{
    public static SecretsManagerConfigurationException? ConnectionException { get; set; }

    public static SecretsManagerConnectionStateEnum ConnectionState { get; set; } =
        SecretsManagerConnectionStateEnum.Connected;
}
