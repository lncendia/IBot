using IBot.Core.Interfaces.Repositories;
using IBot.DAL.Context;

namespace IBot.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        UserRepository = new Lazy<IUserRepository>(() => new UserRepository(_dbContext));
        ProductRepository = new Lazy<IProductRepository>(() => new ProductRepository(_dbContext));
        TransactionRepository = new Lazy<ITransactionRepository>(() => new TransactionRepository(_dbContext));
    }

    public Lazy<IUserRepository> UserRepository { get; }
    public Lazy<IProductRepository> ProductRepository { get; }
    public Lazy<ITransactionRepository> TransactionRepository { get; }

    public Task SaveAsync() => _dbContext.SaveChangesAsync();
}