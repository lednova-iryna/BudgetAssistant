using System;
using MediatR;
using Assistants.Budget.BE.Domain;

namespace Assistants.Budget.BE.Mediator.Transactions;

public class TransactionsQuery : IRequest<IEnumerable<Transaction>>
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public TransactionType? Type { get; set; }

}

