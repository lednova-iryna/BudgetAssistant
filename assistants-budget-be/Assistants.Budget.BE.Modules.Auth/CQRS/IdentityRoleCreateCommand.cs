using Assistants.Budget.BE.Modules.Auth.Domain;
using Assistants.Budget.BE.Modules.Core;
using FluentValidation;
using MediatR;

namespace Assistants.Budget.BE.Modules.Auth.CQRS;

public class IdentityRoleCreateCommand : IRequest<IdentityRole>
{
    public string Name { get; set; }
    public IEnumerable<string> Permissions { get; set; }

    private class Validator : AbstractValidator<IdentityRoleCreateCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Permissions).NotEmpty();
        }
    }

    private class Handler : IRequestHandler<IdentityRoleCreateCommand, IdentityRole>
    {
        private readonly IdentityService identityService;
        private readonly IRequestIdentityService requestIdentity;

        public Handler(IdentityService identityService, IRequestIdentityService requestIdentity)
        {
            this.identityService = identityService;
            this.requestIdentity = requestIdentity;
        }

        public async Task<IdentityRole> Handle(IdentityRoleCreateCommand request, CancellationToken cancellationToken)
        {
            await new Validator().ValidateAndThrowAsync(request, cancellationToken);
            return await identityService.CreateIdentityRole(
                request,
                requestIdentity.GetUserId().GetValueOrDefault(),
                cancellationToken
            );
        }
    }
}
