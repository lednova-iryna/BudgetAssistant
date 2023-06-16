using Assistants.Extensions.Options;
using FluentValidation;

namespace Assistants.Budget.BE.Modules.Auth.Options;

class AuthOptions : BaseOptions
{
    public override string SectionName => "Auth";

    public string Audience { get; set; }
    public string Authority { get; set; }
    public string OAuth2TokenUrl { get; set; }

    /// <summary>
    /// DEVELOPMENT ONLY!
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// DEVELOPMENT ONLY!
    /// </summary>
    public string? ClientSecret { get; set; }

    public class Validator : AbstractValidator<AuthOptions>
    {
        public Validator()
        {
            RuleFor(x => x.Audience).NotEmpty();
            RuleFor(x => x.Authority).NotEmpty();
            RuleFor(x => x.OAuth2TokenUrl).NotEmpty();
        }
    }
}
