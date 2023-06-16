using Assistants.Budget.BE.Modules.Auth.Domain;
using MediatR;

namespace Assistants.Budget.BE.Modules.Auth.CQRS;

public class IdentityRoleQuery : IRequest<IEnumerable<IdentityRole>>
{
    private class Handler : IRequestHandler<IdentityRoleQuery, IEnumerable<IdentityRole>>
    {
        private readonly IdentityService identityService;

        public Handler(IdentityService identityService)
        {
            this.identityService = identityService;
        }

        public async Task<IEnumerable<IdentityRole>> Handle(
            IdentityRoleQuery request,
            CancellationToken cancellationToken
        )
        {
            return await identityService.GetIdentityRoles(request, cancellationToken);
        }
    }
}
