namespace IBot.Core.Entities.Transactions;

public class Transaction
{
    public Transaction(decimal cost, Guid userId, DateTime date)
    {
        Id = Guid.NewGuid();
        Cost = cost;
        UserId = userId;
        Date = date;
    }

    public Guid Id { get; }
    public decimal Cost { get; }
    public Guid UserId { get; }
    public DateTime Date { get; }
}