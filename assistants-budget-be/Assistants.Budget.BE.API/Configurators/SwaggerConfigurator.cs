using System;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Assistants.Budget.BE.Options;
using Assistants.Extensions.Options;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Any;
using Microsoft.Extensions.Options;

namespace Assistants.Budget.BE.API.Configurators;

public static class SwaggerConfigurator
{
    public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var generalOptions = OptionsExtensions.LoadOptions<GeneralOptions, GeneralOptions.Validator>(configuration);

        if (!generalOptions.IsSwaggerEnabled)
            return;

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo { Title = "Assistants: Budget API", Version = "v1" }
            );
            options.AddServer(new OpenApiServer
            {
                Url = "/"
            });

            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                }
            );

            options.IncludeXmlComments(
                Path.Combine(
                    AppContext.BaseDirectory,
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"
                )
            );

            options.AddSecurityDefinition("oauth2",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri($"/auth/token", UriKind.Relative),

                        }
                    },
                    Description = "API"
                });
            //options.IncludeXmlComments(
            //    Path.Combine(
            //        AppContext.BaseDirectory,
            //        $"{typeof(ErrorCodes).Assembly.GetName().Name}.xml"));




            //options.AddSecurityDefinition(Policies.AdministrativeApi,
            //    new OpenApiSecurityScheme
            //    {
            //        Type = SecuritySchemeType.OAuth2,
            //        Flows = new OpenApiOAuthFlows
            //        {
            //            ClientCredentials = new OpenApiOAuthFlow
            //            {
            //                TokenUrl = new Uri("/connect/token", UriKind.Relative),
            //                Scopes =
            //                {
            //        { Scopes.CoreApi, "Core API Scope" },
            //        { Scopes.AdministrativeApi, "Administrative API Scope" }
            //                }
            //            }
            //        },
            //        Description = "Administrative API"
            //    });



            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { 
                    new OpenApiSecurityScheme
                    {
                        Name = "Bearer",
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        services.AddSwaggerGenNewtonsoftSupport();
    }

    public static void UseSwagger(this WebApplication app, bool isEnabled)
    {
        if (!isEnabled)
            return;

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/swagger/v1/swagger.json", "Assistants: Budget API");
        });
    }
}
