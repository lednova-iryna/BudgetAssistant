using Assistants.Budget.BE.Modules.Auth.Models;
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

        public Handler(AuthService authService)
        {
            this.authService = authService;
        }

        public async Task<AuthTokens> Handle(ClientCredentialsTokenQuery request, CancellationToken cancellationToken)
        {
            await new Validator().ValidateAndThrowAsync(request, cancellationToken);
            var accessToken = await authService.GetTokenAsync(
                request.ClientId,
                request.ClientSecret,
                cancellationToken
            );
            return new AuthTokens(accessToken);
        }
    }
}
