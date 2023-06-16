using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Amazon.Extensions.NETCore.Setup;
using Assistants.Libs.Aws.Parameters.Constants;

namespace Assistants.Libs.Aws.Parameters;

static class ParameterStoreHelper
{
    public static IConfigurationBuilder AddAWSParameterStore(
        IConfigurationBuilder configurationBuilder,
        List<string> parameters
    )
    {
        StringBuilder errMsg = new StringBuilder();

        AWSOptions options = new AWSOptions();
        AwsCredentialsLocator.AwsEndpointCredentials awsEndpointCredentials = AwsCredentialsLocator.LocateCredentials();

        options.Credentials = awsEndpointCredentials.Credentials;
        options.Region = awsEndpointCredentials.RegionEndpoint;

        foreach (string parameter in parameters)
        {
            try
            {
                configurationBuilder.AddSystemsManager(Path.Combine("/", parameter), options, false);
            }
            catch (Exception exception)
            {
                errMsg.AppendLine($@"Unable to fetch parameter {parameter}, {exception.InnerException.Message}");
            }
        }

        if (errMsg.Length > 0)
        {
            AwsParametersConnectionState.ConnectionState = SecretsManagerConnectionStateEnum.ConnectionError;
            AwsParametersConnectionState.ConnectionException = new(errMsg.ToString(), new Exception());
        }

        return configurationBuilder;
    }
}
