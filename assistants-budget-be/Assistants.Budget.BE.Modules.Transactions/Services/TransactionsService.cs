using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Assistants.Budget.BE.Modules.Database.Options;
using Assistants.Budget.BE.Modules.Transactions.CQRS;
using Assistants.Budget.BE.Modules.Transactions.Domain;
using Assistants.Budget.BE.Modules.Transactions.Validators;
using FluentValidation;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Assistants.Budget.BE.Modules.Transactions.Services;

internal class TransactionsService
{
    private readonly MongoClient mongoClient;
    private readonly ILogger logger;
    private readonly DatabaseOptions databaseOptions;

    private IMongoCollection<Transaction> GetCollection()
    {
        return mongoClient.GetDatabase(databaseOptions.Name).GetCollection<Transaction>(nameof(Transaction));
    }

    public TransactionsService(
        MongoClient mongoClient,
        IOptions<DatabaseOptions> options,
        ILogger<TransactionsService> logger
    )
    {
        this.mongoClient = mongoClient;
        this.logger = logger;
        this.databaseOptions = options.Value;
    }

    public async Task<Transaction> Create(
        TransactionsCreateCommand command,
        Guid createdBy,
        CancellationToken cancellationToken
    )
    {
        var document = new Transaction(
            Id: Guid.NewGuid(),
            Amount: command.Amount,
            Date: command.Date,
            Note: command.Note,
            Type: command.Type,
            CreatedBy: createdBy,
            CreatedAt: DateTime.UtcNow
        );

        try
        {
            new TransactionValidator().ValidateAndThrow(document);
        }
        catch (ValidationException ex)
        {
            logger.LogError(ex, "Unable to save transaction. Validation failed: ");
            throw;
        }

        await GetCollection().InsertOneAsync(document, cancellationToken: cancellationToken);

        return document;
    }

    public async Task<Transaction> GetById(Guid id, CancellationToken cancellationToken) =>
        await GetCollection().Find(x => x.Id == id).Limit(1).FirstOrDefaultAsync(cancellationToken);

    public async Task<IEnumerable<Transaction>> Get(TransactionsQuery query, CancellationToken cancellationToken)
    {
        var dbQuery = GetCollection().AsQueryable().Where(x => x.Date >= query.FromDate && x.Date <= query.ToDate);

        if (query.Type.HasValue)
        {
            dbQuery = dbQuery.Where(x => x.Type == query.Type);
        }

        return await dbQuery.ToListAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await GetCollection().DeleteOneAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }
}
