using System;
using Assistants.Budget.BE.BusinessLogic.Transactions.CQRS;
using Assistants.Budget.BE.Domain;
using Assistants.Budget.BE.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Assistants.Budget.BE.BusinessLogic.Transactions;


public class TransactionsService
{
    private readonly MongoClient mongoClient;
    private readonly DatabaseOptions databaseOptions;

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

        await mongoClient
            .GetDatabase(databaseOptions.Name)
            .GetCollection<Transaction>(nameof(Transaction))
            .InsertOneAsync(document, cancellationToken: cancellationToken);

        return document;
    }
}


