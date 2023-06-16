using System;
using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Assistants.Libs.Aws.Parameters.Constants;

namespace Assistants.Libs.Aws.Parameters;

public class AwsCredentialsLocator
{
    public class AwsEndpointCredentials
    {
        public AWSCredentials? Credentials { get; set; }
        public RegionEndpoint? RegionEndpoint { get; set; }
    }

    public static AwsEndpointCredentials LocateCredentials()
    {
        AwsEndpointCredentials result = new AwsEndpointCredentials();
        try
        {
            result.Credentials = AssumeRoleWithWebIdentityCredentials.FromEnvironmentVariables();
        }
        catch (Exception ex)
        {
            // ignored
        }

        if (result.Credentials == null)
        {
            string profileName =
                Environment.GetEnvironmentVariable(AwsEnvironmentVariables.AwsProfile)
                ?? AwsEnvironmentVariables.AwsDefaultProfileName;

            if (new CredentialProfileStoreChain().TryGetProfile(profileName, out CredentialProfile? profile))
            {
                result.Credentials = profile.GetAWSCredentials(profile.CredentialProfileStore);
                result.RegionEndpoint = profile.Region;
            }
            else if (
                Environment.GetEnvironmentVariable(AwsEnvironmentVariables.AwsAccessKeyId) != null
                && Environment.GetEnvironmentVariable(AwsEnvironmentVariables.AwsSecretAccessKey) != null
            )
            {
                result.Credentials = new EnvironmentVariablesAWSCredentials();
            }
            else
            {
                try
                {
                    result.Credentials = new InstanceProfileAWSCredentials();
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
        }

        string? region = Environment.GetEnvironmentVariable(AwsEnvironmentVariables.AwsDefaultRegion) ?? "us-east-1";

        if (region != null)
        {
            result.RegionEndpoint = RegionEndpoint.GetBySystemName(region) ?? RegionEndpoint.USEast1;
        }

        return result;
    }
}
