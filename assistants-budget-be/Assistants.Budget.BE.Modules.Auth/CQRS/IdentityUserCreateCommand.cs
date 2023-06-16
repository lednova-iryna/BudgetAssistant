using Assistants.Budget.BE.Modules.Auth.Domain;
using Assistants.Budget.BE.Modules.Core;
using FluentValidation;
using MediatR;

namespace Assistants.Budget.BE.Modules.Auth.CQRS;

public class IdentityUserCreateCommand : IRequest<IdentityUser>
{
    public string UserName { get; set; }
    public IEnumerable<Guid>? Roles { get; set; }

    private class Validator : AbstractValidator<IdentityUserCreateCommand>
    {
        public Validator()
        {
            RuleFor(x => x.UserName).NotEmpty();
        }
    }

    private class Handler : IRequestHandler<IdentityUserCreateCommand, IdentityUser>
    {
        private readonly IdentityService identityService;
        private readonly IRequestIdentityService requestIdentity;

        public Handler(IdentityService identityService, IRequestIdentityService requestIdentity)
        {
            this.identityService = identityService;
            this.requestIdentity = requestIdentity;
        }

        public async Task<IdentityUser> Handle(IdentityUserCreateCommand request, CancellationToken cancellationToken)
        {
            await new Validator().ValidateAndThrowAsync(request, cancellationToken);
            if (request.Roles != null)
                foreach (var roleID in request.Roles)
                {
                    if (await identityService.GetIdentityRoleById(roleID, cancellationToken) == null)
                    {
                        throw new ValidationException($"Role with Id {roleID} not found");
                    }
                }

            return await identityService.CreateIdentityUser(
                request,
                requestIdentity.GetUserId().GetValueOrDefault(),
                cancellationToken
            );
        }
    }
}
