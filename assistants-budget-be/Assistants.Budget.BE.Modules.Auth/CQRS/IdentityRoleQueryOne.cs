using Assistants.Budget.BE.Modules.Auth.Domain;
using FluentValidation;
using MediatR;

namespace Assistants.Budget.BE.Modules.Auth.CQRS;

public class IdentityRoleQueryOne : IRequest<IdentityRole>
{
    public Guid Id { get; set; }

    private class Validator : AbstractValidator<IdentityRoleQueryOne>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }

    private class Handler : IRequestHandler<IdentityRoleQueryOne, IdentityRole>
    {
        private readonly IdentityService identityService;

        public Handler(IdentityService identityService)
        {
            this.identityService = identityService;
        }

        public async Task<IdentityRole> Handle(IdentityRoleQueryOne request, CancellationToken cancellationToken)
        {
            await new Validator().ValidateAndThrowAsync(request, cancellationToken);
            return await identityService.GetIdentityRoleById(request.Id, cancellationToken);
        }
    }
}
