namespace IBot.Core.Interfaces.Services.DTOs;

public class TransactionDto
{
    public TransactionDto(decimal amount, DateTime payTime, Guid userId)
    {
        Amount = amount;
        PayTime = payTime;
        UserId = userId;
    }
    
    public decimal Amount { get; }
    public DateTime PayTime { get; }
    public Guid UserId { get; }
}