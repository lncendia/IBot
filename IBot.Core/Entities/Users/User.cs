using IBot.Core.Entities.Products;
using IBot.Core.Entities.Transactions;
using IBot.Core.Entities.Users.Enums;
using IBot.Core.Entities.Users.Exceptions;

namespace IBot.Core.Entities.Users;

public class User
{
    public User(long telegramId)
    {
        Id = Guid.NewGuid();
        TelegramId = telegramId;
    }

    public Guid Id { get; }

    public long TelegramId { get; }
    public State State { get; set; } = State.Main;
    public bool IsBanned { get; set; }
    public bool IsAdmin { get; set; }
    public string? TempProductPreview { get; set; }
    public string? TempProduct { get; set; }
    public decimal Balance { get; private set; }

    private readonly List<Guid> _products = new();
    private readonly List<Guid> _transactions = new();

    public List<Guid> Products => _products.ToList();
    public List<Guid> Transactions => _transactions.ToList();

    public void BuyProduct(Product product)
    {
        if (Balance < product.Cost) throw new NotEnoughMoneyException();
        Balance -= product.Cost;
        _products.Add(product.Id);
    }

    public void PerformTransaction(Transaction transaction)
    {
        Balance += transaction.Cost;
        _transactions.Add(transaction.Id);
    }
}