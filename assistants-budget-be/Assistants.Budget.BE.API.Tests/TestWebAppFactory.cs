using System;
using dotenv.net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Assistants.Budget.BE.API.Tests;

public class TestWebAppFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(c =>
        {
            DotEnv.Load(new DotEnvOptions(envFilePaths: new List<string> { "./.env" }));
        });
    }
}
