using Assistants.Budget.BE.Modules.Auth.Domain;
using MediatR;

namespace Assistants.Budget.BE.Modules.Auth.CQRS;

public class IdentityRoleQuery : IRequest<IEnumerable<IdentityRole>>
{
    private class Handler : IRequestHandler<IdentityRoleQuery, IEnumerable<IdentityRole>>
    {
        private readonly AuthService authService;

        public Handler(AuthService identityService)
        {
            this.authService = identityService;
        }

        public async Task<IEnumerable<IdentityRole>> Handle(
            IdentityRoleQuery request,
            CancellationToken cancellationToken
        )
        {
            var authRoles = await authService.GetRoles(cancellationToken);
            return authRoles.Select(x => new IdentityRole(x.Id, x.Name, x.Description));
        }
    }
}
