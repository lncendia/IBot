using IBot.Core.Entities.Users;

namespace IBot.DAL.Models;

public class UserTransactionModel
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Guid TransactionId { get; set; }
}