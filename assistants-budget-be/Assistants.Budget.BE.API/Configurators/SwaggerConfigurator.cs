using System;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Assistants.Budget.BE.API.Configurators;

public static class SwaggerConfigurator
{
    public static void AddSwagger(this IServiceCollection services, bool isEnables)
    {
        if (!isEnables)
            return;

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo { Title = "Assistants: Budget API", Version = "v1" }
            );

            //options.IncludeXmlComments(
            //    Path.Combine(
            //        AppContext.BaseDirectory,
            //        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"
            //    )
            //);

            //options.IncludeXmlComments(
            //    Path.Combine(
            //        AppContext.BaseDirectory,
            //        $"{typeof(ErrorCodes).Assembly.GetName().Name}.xml"));


            //options.AddSecurityDefinition(Policies.CoreApi,
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
            //        {
            //            Scopes.CoreApi, "Core API Scope"
            //        }
            //                }
            //            }
            //        },
            //        Description = "Core API"
            //    });

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

            //options.AddSecurityDefinition(
            //    "Bearer",
            //    new OpenApiSecurityScheme
            //    {
            //        In = ParameterLocation.Header,
            //        Description = "Please enter a valid token",
            //        Name = "Authorization",
            //        Type = SecuritySchemeType.Http,
            //        BearerFormat = "JWT",
            //        Scheme = "Bearer"
            //    }
            //);

            //options.AddSecurityRequirement(new OpenApiSecurityRequirement
            //{
            //    {
            //        new OpenApiSecurityScheme
            //        {
            //            Reference = new OpenApiReference
            //            {
            //                Type = ReferenceType.SecurityScheme,
            //                Id = Policies.CoreApi
            //            }
            //        },
            //        new[] { Scopes.CoreApi }
            //    },
            //    {
            //        new OpenApiSecurityScheme
            //        {
            //            Reference = new OpenApiReference
            //            {
            //                Type = ReferenceType.SecurityScheme,
            //                Id = Policies.AdministrativeApi
            //            }
            //        },
            //        new[] { Scopes.CoreApi, Scopes.AdministrativeApi }
            //    },
            //    {
            //        new OpenApiSecurityScheme
            //        {
            //            Reference = new OpenApiReference
            //            {
            //                Type = ReferenceType.SecurityScheme,
            //                Id = "Bearer"
            //            }
            //        },
            //        new string[] { }
            //    }
            //});
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
