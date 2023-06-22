using Assistants.Budget.BE.Modules.Auth.Domain;
using Assistants.Budget.BE.Modules.Core;
using Auth0.ManagementApi.Models;
using FluentValidation;
using MediatR;

namespace Assistants.Budget.BE.Modules.Auth.CQRS;

public class IdentityUserCreateCommand : IRequest<IdentityUser>
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public IEnumerable<string>? Roles { get; set; }

    private class Validator : AbstractValidator<IdentityUserCreateCommand>
    {
        public Validator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }

    private class Handler : IRequestHandler<IdentityUserCreateCommand, IdentityUser>
    {
        private readonly IdentityService identityService;
        private readonly AuthService authService;
        private readonly IRequestIdentityService requestIdentity;

        public Handler(
            IdentityService identityService,
            AuthService authService,
            IRequestIdentityService requestIdentity
        )
        {
            this.identityService = identityService;
            this.authService = authService;
            this.requestIdentity = requestIdentity;
        }

        public async Task<IdentityUser> Handle(IdentityUserCreateCommand request, CancellationToken cancellationToken)
        {
            await new Validator().ValidateAndThrowAsync(request, cancellationToken);

            var user = await authService.CreateUserAsync(
                new UserCreateRequest
                {
                    UserName = request.UserName,
                    Password = request.Password,
                    Email = request.Email,
                    Blocked = false,
                    Connection = "Username-Password-Authentication",
                    EmailVerified = true
                },
                cancellationToken
            );

            if (request.Roles != null)
            {
                await authService.UserAssignRolesAsync(
                    user.UserId,
                    new AssignRolesRequest { Roles = request.Roles.ToArray() },
                    cancellationToken
                );
            }

            return await identityService.CreateIdentityUser(
                user.UserId,
                request,
                requestIdentity.GetUserId().GetValueOrDefault(),
                cancellationToken
            );
        }
    }
}
