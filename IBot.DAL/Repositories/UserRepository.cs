using System.Reflection;
using AutoMapper;
using IBot.Core.Entities.Users;
using IBot.Core.Interfaces.Repositories;
using IBot.DAL.Context;
using IBot.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace IBot.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
        _mapper = GetMapper();
    }

    private User Map(UserModel model)
    {
        var user = _mapper.Map<User>(model);
        var x = user.GetType();
        x.GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(user, model.Id);
        x.GetField("_products", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(user,
            model.Products.Select(p => p.ProductId).ToList());
        x.GetField("_transactions", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(user,
            model.Transactions.Select(p => p.TransactionId).ToList());

        return user;
    }

    private void UpdateMap(UserModel model, User user)
    {
        var oldTransactions = new List<UserTransactionModel>();
        var newTransactions = new List<UserTransactionModel>();
        var transactions = user.Transactions;
        foreach (var transaction in transactions)
        {
            var transactionModel = model.Transactions.FirstOrDefault(t => t.TransactionId == transaction);
            if (transactionModel == null)
                newTransactions.Add(new UserTransactionModel
                {
                    TransactionId = transaction,
                    UserId = model.Id
                });
            else oldTransactions.Add(transactionModel);
        }

        _context.RemoveRange(model.Transactions.Where(x => !oldTransactions.Contains(x)));
        model.Transactions.Clear();
        model.Transactions.AddRange(oldTransactions);
        model.Transactions.AddRange(newTransactions);

        var oldProducts = new List<UserProductModel>();
        var newProducts = new List<UserProductModel>();
        var products = user.Products;
        foreach (var product in products)
        {
            var productModel = model.Products.FirstOrDefault(t => t.ProductId == product);
            if (productModel == null)
                newProducts.Add(new UserProductModel
                {
                    ProductId = product,
                    UserId = model.Id
                });
            else oldProducts.Add(productModel);
        }

        _context.RemoveRange(model.Products.Where(x => !oldProducts.Contains(x)));
        model.Products.Clear();
        model.Products.AddRange(oldProducts);
        model.Products.AddRange(newProducts);
        _mapper.Map(user, model);
    }

    public async Task<User?> GetAsync(Guid id)
    {
        var user = await _context.Users.Include(u => u.Products).Include(u => u.Transactions)
            .FirstOrDefaultAsync(userModel => userModel.Id == id);
        return user is null ? null : Map(user);
    }

    public async Task AddAsync(User entity)
    {
        var user = new UserModel();
        UpdateMap(user, entity);
        await _context.Users.AddAsync(user);
    }

    public Task DeleteAsync(User entity)
    {
        var user = _context.Users.First(userModel => userModel.Id == entity.Id);
        _context.Users.Remove(user);
        return Task.CompletedTask;
    }

    public async Task UpdateAsync(User entity)
    {
        var user = await _context.Users.Include(u => u.Products).Include(u => u.Transactions)
            .FirstAsync(userModel => userModel.Id == entity.Id);
        UpdateMap(user, entity);
    }

    public async Task<List<User>> Find(int skip, int take)
    {
        var users = await _context.Users.Include(u => u.Products).Include(u => u.Transactions).OrderBy(x => x.Id)
            .Skip(skip).Take(take)
            .ToListAsync();
        return users.Select(Map).ToList();
    }

    public async Task<User?> GetByTelegramIdAsync(long id)
    {
        var user = await _context.Users.Include(u => u.Products).Include(u => u.Transactions)
            .FirstOrDefaultAsync(userModel => userModel.TelegramId == id);
        return user is null ? null : Map(user);
    }

    private static IMapper GetMapper() => new Mapper(new MapperConfiguration(expr =>
    {
        expr.CreateMap<User, UserModel>().ForMember(x => x.Products, expression => expression.Ignore())
            .ForMember(x => x.Transactions, expression => expression.Ignore());
        expr.CreateMap<UserModel, User>().ForMember(x => x.Products, expression => expression.Ignore())
            .ForMember(x => x.Transactions, expression => expression.Ignore());
    }));
}