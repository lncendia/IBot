namespace IBot.DAL.Models;

public class TransactionModel
{
    public Guid Id { get; set; }
    public decimal Cost { get; set; }
    public Guid UserId { get; set; }
    public string Wallet { get; set; } = null!;
    public string Service { get; set; } = null!;
}