namespace IBot.Core.Interfaces.DTOs;

public class TransactionDto
{
    public TransactionDto(string id, decimal amount, long payTime, string signature, string payService, string payerDetails, Guid userId)
    {
        Id = id;
        Amount = amount;
        PayTime = payTime;
        Signature = signature;
        PayService = payService;
        PayerDetails = payerDetails;
        UserId = userId;
    }

    public string Id { get; }
    public decimal Amount { get; }
    public long PayTime { get; }
    public string Signature { get; }
    public string PayService { get; }
    public string PayerDetails { get; }
    public Guid UserId { get; }
}