using Assistants.Budget.BE.BusinessLogic.Transactions.CQRS;
using Assistants.Budget.BE.Domain;
using Assistants.Budget.BE.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Assistants.Budget.BE.BusinessLogic.Transactions;


public class TransactionsService
{
    private readonly MongoClient mongoClient;
    private readonly DatabaseOptions databaseOptions;

    private IMongoCollection<Transaction> GetCollection()
    {
        return mongoClient
           .GetDatabase(databaseOptions.Name)
           .GetCollection<Transaction>(nameof(Transaction));
    }

    public TransactionsService(MongoClient mongoClient, IOptions<DatabaseOptions> options)
    {
        this.mongoClient = mongoClient;
        this.databaseOptions = options.Value;
    }

    public async Task<Transaction> Create(TransactionsCreateCommand command, CancellationToken cancellationToken)
    {
        var document = new Transaction
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Amount = command.Amount,
            Date = command.Date,
            Note = command.Note,
            Type = command.Type
        };

        await GetCollection()
            .InsertOneAsync(document, cancellationToken: cancellationToken);

        return document;
    }

    public async Task<Transaction> GetById(Guid id, CancellationToken cancellationToken)
        => await GetCollection()
            .Find(x => x.Id == id)
            .Limit(1)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IEnumerable<Transaction>> Get(TransactionsQuery query, CancellationToken cancellationToken)
    {
        var dbQuery = GetCollection()
             .AsQueryable()
             .Where(x => x.Date >= query.FromDate && x.Date <= query.ToDate);

        if (query.Type.HasValue)
        {
            dbQuery = dbQuery.Where(x => x.Type == query.Type);
        }

        return await dbQuery.ToListAsync(cancellationToken);
    }
}


