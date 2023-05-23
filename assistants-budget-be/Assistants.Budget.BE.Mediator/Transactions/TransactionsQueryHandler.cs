using System;
using Assistants.Budget.BE.Domain;
using MediatR;

namespace Assistants.Budget.BE.Mediator.Transactions;

public class TransactionsQueryHandler : IRequestHandler<TransactionsQuery, IEnumerable<Transaction>>
{
    public TransactionsQueryHandler()
    {
    }

    public Task<IEnumerable<Transaction>> Handle(
         TransactionsQuery request,
         CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new List<Transaction>() { new Transaction {
            Amount = 123
        } }.AsEnumerable());
    }
}

