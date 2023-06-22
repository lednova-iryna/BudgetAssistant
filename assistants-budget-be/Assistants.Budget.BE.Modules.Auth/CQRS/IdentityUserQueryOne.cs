using Assistants.Budget.BE.Modules.Auth.Domain;
using FluentValidation;
using MediatR;

namespace Assistants.Budget.BE.Modules.Auth.CQRS;

public class IdentityUserQueryOne : IRequest<IdentityUser>
{
    public string Id { get; set; }

    private class Validator : AbstractValidator<IdentityUserQueryOne>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }

    private class Handler : IRequestHandler<IdentityUserQueryOne, IdentityUser>
    {
        private readonly IdentityService identityService;

        public Handler(IdentityService identityService)
        {
            this.identityService = identityService;
        }

        public async Task<IdentityUser> Handle(IdentityUserQueryOne request, CancellationToken cancellationToken)
        {
            await new Validator().ValidateAndThrowAsync(request, cancellationToken);
            return await identityService.GetIdentityUserById(request.Id, cancellationToken);
        }
    }
}
