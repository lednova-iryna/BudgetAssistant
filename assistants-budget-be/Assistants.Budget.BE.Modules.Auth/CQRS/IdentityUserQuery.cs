using Assistants.Budget.BE.Modules.Auth.Domain;
using MediatR;

namespace Assistants.Budget.BE.Modules.Auth.CQRS;

public class IdentityUserQuery : IRequest<IEnumerable<IdentityUser>>
{
    public IEnumerable<Guid>? Roles { get; set; }
    public IdentityUserStatus? Status { get; set; }

    private class Handler : IRequestHandler<IdentityUserQuery, IEnumerable<IdentityUser>>
    {
        private readonly IdentityService identityService;

        public Handler(IdentityService identityService)
        {
            this.identityService = identityService;
        }

        public async Task<IEnumerable<IdentityUser>> Handle(
            IdentityUserQuery request,
            CancellationToken cancellationToken
        )
        {
            return await identityService.GetIdentityUsers(request, cancellationToken);
        }
    }
}
