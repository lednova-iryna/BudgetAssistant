using Assistants.Extensions.Options;
using FluentValidation;
using MongoDB.Driver;

namespace Assistants.Libs.Caching.MongoDistributedCache;

public class MongoDistributedCacheOptions : BaseOptions
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }
    public TimeSpan? ExpiredScanInterval { get; set; }
    public MongoClientSettings MongoClientSettings { get; set; }

    public override string SectionName => "MongoDistributedCache";

    public class Validator : AbstractValidator<MongoDistributedCacheOptions>
    {
        public Validator()
        {
            RuleFor(x => x.ConnectionString).NotEmpty();
            RuleFor(x => x.DatabaseName).NotEmpty();
            RuleFor(x => x.CollectionName).NotEmpty();
            RuleFor(x => x.ExpiredScanInterval).NotEmpty();
        }
    }
}
