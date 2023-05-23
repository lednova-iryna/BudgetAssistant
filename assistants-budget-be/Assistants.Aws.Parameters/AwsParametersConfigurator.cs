using System.Collections.Generic;
using System.Linq;
using Assistants.Aws.Parameters.Options;
using Assistants.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace Assistants.Libs.Aws.Parameters;

public static class AwsParametersConfigurator
{
    //public static void ConfigureAwsParametersOptions(this IConfigurationBuilder builder)
    //{
    //    var awsOptions = OptionsExtensions.LoadOptions<AwsOptions, AwsOptionsValidator>(
    //        builder.Build()
    //    );

    //    if (!awsOptions.Parameters!.Ignore)
    //    {
    //        builder.AddSystemsManager(s =>
    //        {
    //            s.Path = awsOptions.Parameters.Name;
    //            s.ReloadAfter = TimeSpan.FromSeconds(awsOptions.Parameters.SecretPollingInterval);
    //        });
    //    }
    //}

    public static IConfigurationBuilder AddAwsParameterStore(
        this IConfigurationBuilder configurationBuilder
    )
    {
        var awsOptions = OptionsExtensions.LoadOptions<AwsOptions, AwsOptionsValidator>(
            configurationBuilder.Build()
        );
        if (awsOptions.Parameters.Ignore)
            return configurationBuilder;

        return ParameterStoreHelper.AddAWSParameterStore(
            configurationBuilder,
            awsOptions.Parameters.Names.Split(";").ToList()
        );
    }
}
