using System;

namespace Assistants.Libs.Aws.Parameters;

public class SecretsManagerConfigurationException : Exception
{
    public SecretsManagerConfigurationException(string errorMessage, Exception exception)
        : base(errorMessage, exception) { }
}
