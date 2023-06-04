using System;
using MediatR;
using Assistants.Budget.BE.Domain;

namespace Assistants.Budget.BE.BusinessLogic.Transactions.CQRS;

public class TransactionsQuery : IRequest<IEnumerable<Transaction>>
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public TransactionType? Type { get; set; }
    public Guid? Id { get; set; }

    public class Handler : IRequestHandler<TransactionsQuery, IEnumerable<Transaction>>
    {
        private readonly TransactionsService transactionsService;

        public Handler(TransactionsService transactionsService)
        {
            this.transactionsService = transactionsService;
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
}

