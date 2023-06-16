using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using dotenv.net;

namespace Assistants.Budget.BE.API.Tests.Mocks;

public class WebAppFactoryMock<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        IdentityModelEventSource.ShowPII = true;

        builder.ConfigureAppConfiguration(c =>
        {
            DotEnv.Load(new DotEnvOptions(envFilePaths: new List<string> { "./.env" }));
        });
        builder.AddAuthModuleMock();
    }
}
