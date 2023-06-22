using Assistants.Extensions.Options;
using FluentValidation;

namespace Assistants.Budget.BE.Modules.Auth.Options;

class AuthOptions : BaseOptions
{
    public override string SectionName => "Auth";

    public string Audience { get; set; }
    public string Authority { get; set; }
    public string OAuth2TokenUrl { get; set; }
    public string ManagementApiAudience { get; set; }
    public string ManagementApiClientId { get; set; }
    public string ManagementApiClientSecret { get; set; }

    /// <summary>
    /// DEVELOPMENT ONLY!
    /// </summary>
    public string? TestClientId { get; set; }

    /// <summary>
    /// DEVELOPMENT ONLY!
    /// </summary>
    public string? TestClientSecret { get; set; }

    public class Validator : AbstractValidator<AuthOptions>
    {
        public Validator()
        {
            RuleFor(x => x.Audience).NotEmpty();
            RuleFor(x => x.Authority).NotEmpty();
            RuleFor(x => x.OAuth2TokenUrl).NotEmpty();
            RuleFor(x => x.ManagementApiAudience).NotEmpty();
            RuleFor(x => x.ManagementApiClientId).NotEmpty();
            RuleFor(x => x.ManagementApiClientSecret).NotEmpty();
        }
    }
}
