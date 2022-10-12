namespace IBot.DAL.Models;

public class TransactionModel
{
    public Guid Id { get; set; }
    public decimal Cost { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
}