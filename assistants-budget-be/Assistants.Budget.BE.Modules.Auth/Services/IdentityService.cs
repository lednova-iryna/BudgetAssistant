using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Assistants.Budget.BE.Modules.Auth.CQRS;
using Assistants.Budget.BE.Modules.Auth.Domain;
using Assistants.Budget.BE.Modules.Database.Options;
using MongoDB.Driver;

namespace Assistants.Budget.BE.Modules.Auth;

class IdentityService
{
    private readonly MongoClient mongoClient;
    private readonly AuthService authService;
    private readonly ILogger logger;
    private readonly DatabaseOptions databaseOptions;

    private IMongoCollection<IdentityRole> GetRolesCollection()
    {
        return mongoClient.GetDatabase(databaseOptions.Name).GetCollection<IdentityRole>(nameof(IdentityRole));
    }

    private IMongoCollection<IdentityUser> GetUsersCollection()
    {
        return mongoClient.GetDatabase(databaseOptions.Name).GetCollection<IdentityUser>(nameof(IdentityUser));
    }

    public IdentityService(
        MongoClient mongoClient,
        AuthService authService,
        IOptions<DatabaseOptions> options,
        ILogger<IdentityService> logger
    )
    {
        this.mongoClient = mongoClient;
        this.authService = authService;
        this.logger = logger;
        this.databaseOptions = options.Value;
    }

    #region IdentityRole
    public async Task<IdentityRole> CreateIdentityRole(
        string roleId,
        IdentityRoleCreateCommand request,
        Guid createdBy,
        CancellationToken cancellationToken
    )
    {
        var document = new IdentityRole(
            Id: roleId,
            Name: request.Name,
            Description: request.Description,
            Permissions: request.Permissions
        );

        await GetRolesCollection().InsertOneAsync(document, cancellationToken: cancellationToken);

        return document;
    }

    public async Task UpdateIdentityRole(
        string roleId,
        IdentityRoleUpdateCommand request,
        Guid modifiedBy,
        CancellationToken cancellationToken
    )
    {
        var updateQuery = Builders<IdentityRole>.Update
            .Set(r => r.Name, request.Name)
            .Set(r => r.Permissions, request.Permissions);
        await GetRolesCollection()
            .UpdateOneAsync(x => x.Id == roleId, updateQuery, cancellationToken: cancellationToken);
    }

    public async Task<IdentityRole> GetIdentityRoleById(string id, CancellationToken cancellationToken) =>
        await GetRolesCollection().Find(x => x.Id == id).Limit(1).FirstOrDefaultAsync(cancellationToken);

    public async Task<IEnumerable<IdentityRole>> GetIdentityRoles(
        IdentityRoleQuery query,
        CancellationToken cancellationToken
    ) => await GetRolesCollection().AsQueryable().ToListAsync(cancellationToken);

    #endregion

    #region IdentityUser

    public async Task<IdentityUser> GetIdentityUserById(string id, CancellationToken cancellationToken) =>
        await GetUsersCollection().Find(x => x.Id == id).Limit(1).FirstOrDefaultAsync(cancellationToken);

    public async Task<IdentityUser> CreateIdentityUser(
        string userId,
        IdentityUserCreateCommand request,
        Guid createdBy,
        CancellationToken cancellationToken
    )
    {
        var document = new IdentityUser(
            Id: userId,
            UserName: request.UserName,
            Roles: request.Roles,
            Status: IdentityUserStatus.Active,
            CreatedBy: createdBy,
            CreatedAt: DateTime.UtcNow
        );

        await GetUsersCollection().InsertOneAsync(document, cancellationToken: cancellationToken);

        return document;
    }

    internal async Task<IEnumerable<IdentityUser>> GetIdentityUsers(
        IdentityUserQuery request,
        CancellationToken cancellationToken
    )
    {
        var dbQuery = GetUsersCollection();
        var filter = Builders<IdentityUser>.Filter.Empty;

        if (request.Roles?.Count() > 0)
        {
            filter &= Builders<IdentityUser>.Filter.AnyIn(x => x.Roles, request.Roles);
        }
        if (request.Status.HasValue)
        {
            filter &= Builders<IdentityUser>.Filter.Eq(x => x.Status, request.Status);
        }

        return await dbQuery.Find(filter).ToListAsync(cancellationToken);
    }

    #endregion
}
