using IBot.Core.Entities.Users.Enums;

namespace IBot.DAL.Models;

public class UserModel
{
    public Guid Id { get; set; }

    public State State { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsBanned { get; set; }
    public long TelegramId { get; set; }
    public decimal Balance { get; set; }
    
    public string? TempProductPreview { get; set; }
    public string? TempProduct { get; set; }

    public List<UserProductModel> Products { get; set; } = new();
    public List<UserTransactionModel> Transactions { get; set; } = new();
}