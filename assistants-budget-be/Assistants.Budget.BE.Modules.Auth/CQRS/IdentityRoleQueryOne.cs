using Assistants.Budget.BE.Modules.Auth.Domain;
using FluentValidation;
using MediatR;

namespace Assistants.Budget.BE.Modules.Auth.CQRS;

public class IdentityRoleQueryOne : IRequest<IdentityRole?>
{
    public string Id { get; set; }

    private class Validator : AbstractValidator<IdentityRoleQueryOne>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }

    private class Handler : IRequestHandler<IdentityRoleQueryOne, IdentityRole?>
    {
        private readonly AuthService authService;

        public Handler(AuthService authService)
        {
            this.authService = authService;
        }

        public async Task<IdentityRole?> Handle(IdentityRoleQueryOne request, CancellationToken cancellationToken)
        {
            await new Validator().ValidateAndThrowAsync(request, cancellationToken);
            var role = await authService.GetRole(request.Id, cancellationToken);
            var permissions = await authService.GetRolePermissions(request.Id, cancellationToken);
            if (role == null)
                return null;
            return new IdentityRole(role.Id, role.Name, role.Description, permissions.Select(x => x.Name));
        }
    }
}
