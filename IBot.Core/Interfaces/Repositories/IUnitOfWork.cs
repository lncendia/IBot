namespace IBot.Core.Interfaces.Repositories;

public interface IUnitOfWork
{
    public Lazy<IUserRepository> UserRepository { get; }
    public Lazy<IProductRepository> ProductRepository { get; }
    public Lazy<ITransactionRepository> TransactionRepository { get; }
    public Task SaveAsync();
}