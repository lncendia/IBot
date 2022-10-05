using System.Reflection;
using AutoMapper;
using IBot.Core.Entities.Transactions;
using IBot.Core.Interfaces.Repositories;
using IBot.DAL.Context;
using IBot.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace IBot.DAL.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
        _mapper = GetMapper();
    }

    private Transaction Map(TransactionModel model)
    {
        var transaction = _mapper.Map<Transaction>(model);
        var x = transaction.GetType();
        x.GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(transaction,
            model.Id);

        return transaction;
    }

    public async Task<Transaction?> GetAsync(Guid id)
    {
        var transaction =
            await _context.Transactions.FirstOrDefaultAsync(transactionModel => transactionModel.Id == id);
        return transaction is null ? null : Map(transaction);
    }

    public async Task AddAsync(Transaction entity)
    {
        var transaction = new TransactionModel();
        _mapper.Map(entity, transaction);
        await _context.Transactions.AddAsync(transaction);
    }

    public Task DeleteAsync(Transaction entity)
    {
        var transaction = _context.Transactions.First(transactionModel => transactionModel.Id == entity.Id);
        _context.Transactions.Remove(transaction);
        return Task.CompletedTask;
    }

    public async Task UpdateAsync(Transaction entity)
    {
        var transaction = await _context.Transactions.FirstAsync(transactionModel => transactionModel.Id == entity.Id);
        _mapper.Map(entity, transaction);
    }

    public async Task<List<Transaction>> Find(int skip, int take)
    {
        var transactions = await _context.Transactions.OrderBy(x => x.Id).Skip(skip).Take(take).ToListAsync();
        return transactions.Select(Map).ToList();
    }

    private static IMapper GetMapper() => new Mapper(new MapperConfiguration(expr =>
    {
        expr.CreateMap<Transaction, TransactionModel>().ReverseMap();
    }));
}