namespace Assistants.Budget.BE.Domain;
public class Transaction
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime Date { get; set; }
    public double Amount { get; set; }
    public string Note { get; set; }
    public TransactionType Type { get; set; }

}

