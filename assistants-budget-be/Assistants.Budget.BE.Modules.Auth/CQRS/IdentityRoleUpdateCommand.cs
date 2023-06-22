using Assistants.Budget.BE.Modules.Auth.Domain;
using Assistants.Budget.BE.Modules.Auth.Options;
using Assistants.Budget.BE.Modules.Core;
using Auth0.ManagementApi.Models;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;

namespace Assistants.Budget.BE.Modules.Auth.CQRS;

public class IdentityRoleUpdateCommand : IRequest
{
    public string RoleId { get; set; }
    public string Name { get; set; }
    public IEnumerable<string> Permissions { get; set; }

    private class Validator : AbstractValidator<IdentityRoleUpdateCommand> { }

    private class Handler : IRequestHandler<IdentityRoleUpdateCommand>
    {
        private readonly IdentityService identityService;
        private readonly AuthService authService;
        private readonly AuthOptions authOptions;
        private readonly IRequestIdentityService requestIdentity;

        public Handler(
            IdentityService identityService,
            AuthService authService,
            IOptions<AuthOptions> authOptions,
            IRequestIdentityService requestIdentity
        )
        {
            this.identityService = identityService;
            this.authService = authService;
            this.authOptions = authOptions.Value;
            this.requestIdentity = requestIdentity;
        }

        public async Task Handle(IdentityRoleUpdateCommand request, CancellationToken cancellationToken)
        {
            await new Validator().ValidateAndThrowAsync(request, cancellationToken);

            var role = await authService.UpdateRoleAsync(
                request.RoleId,
                new RoleUpdateRequest { Name = request.Name, },
                cancellationToken
            );

            await authService.AssignPermissionsToRoleAsync(
                role.Id,
                new AssignPermissionsRequest
                {
                    Permissions = request.Permissions
                        .Select(x => new PermissionIdentity { Name = x, Identifier = authOptions.Audience })
                        .ToList()
                },
                cancellationToken
            );

            await identityService.UpdateIdentityRole(
                role.Id,
                request,
                requestIdentity.GetUserId().GetValueOrDefault(),
                cancellationToken
            );
        }
    }
}
