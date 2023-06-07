namespace Assistants.Aws.Parameters.Constants;

public enum SecretsManagerConnectionStateEnum
{
    None = 0,
    Connected = 1,
    NoCredentialsLocated = 2,
    ConnectionError = 3
}
