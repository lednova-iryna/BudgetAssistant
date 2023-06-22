using Microsoft.Extensions.Options;
using Assistants.Budget.BE.Modules.Auth.Models;
using Assistants.Budget.BE.Modules.Auth.Options;
using FluentValidation;
using MediatR;

namespace Assistants.Budget.BE.Modules.Auth.CQRS;

public class ClientCredentialsTokenQuery : IRequest<AuthTokens>
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }

    private class Validator : AbstractValidator<ClientCredentialsTokenQuery>
    {
        public Validator()
        {
            RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.ClientSecret).NotEmpty();
        }
    }

    private class Handler : IRequestHandler<ClientCredentialsTokenQuery, AuthTokens>
    {
        private readonly AuthService authService;
        private readonly AuthOptions options;

        public Handler(AuthService authService, IOptions<AuthOptions> options)
        {
            this.authService = authService;
            this.options = options.Value;
        }

        public async Task<AuthTokens> Handle(ClientCredentialsTokenQuery request, CancellationToken cancellationToken)
        {
            await new Validator().ValidateAndThrowAsync(request, cancellationToken);
            var accessToken = await authService.GetTokenAsync(
                options.TestClientId ?? request.ClientId,
                options.TestClientSecret ?? request.ClientSecret,
                options.Audience,
                cancellationToken
            );
            return new AuthTokens(accessToken);
        }
    }
}
