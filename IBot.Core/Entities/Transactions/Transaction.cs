namespace IBot.Core.Entities.Transactions;

public class Transaction
{
    public Transaction(decimal cost, Guid userId, string wallet, string service)
    {
        Id = Guid.NewGuid();
        Cost = cost;
        UserId = userId;
        Wallet = wallet;
        Service = service;
    }

    public Guid Id { get; }
    public decimal Cost { get; }
    public Guid UserId { get; }
    public string Service { get; }
    public string Wallet { get; }
}