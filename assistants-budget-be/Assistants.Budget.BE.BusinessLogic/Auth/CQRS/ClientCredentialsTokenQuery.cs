using System;
using Assistants.Budget.BE.Auth0;
using Assistants.Budget.BE.BusinessLogic.Auth.Models;
using Assistants.Budget.BE.BusinessLogic.Transactions.CQRS;
using FluentValidation;
using MediatR;

namespace Assistants.Budget.BE.BusinessLogic.Auth.CQRS;

public class ClientCredentialsTokenQuery : IRequest<AuthTokens>
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }

    public class Validator : AbstractValidator<ClientCredentialsTokenQuery>
    {
        public Validator()
        {
            RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.ClientSecret).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<ClientCredentialsTokenQuery, AuthTokens>
    {
        private readonly Auth0ManagementApiClient auth0ManagementApiClient;

        public Handler(Auth0ManagementApiClient auth0ManagementApiClient)
        {
            this.auth0ManagementApiClient = auth0ManagementApiClient;
        }

        public async Task<AuthTokens> Handle(ClientCredentialsTokenQuery request, CancellationToken cancellationToken)
        {
            await new Validator().ValidateAndThrowAsync(request, cancellationToken);
            var accessToken = await auth0ManagementApiClient.GetTokenAsync(request.ClientId, request.ClientSecret, cancellationToken);
            return new AuthTokens(accessToken);
        }
    }
}

