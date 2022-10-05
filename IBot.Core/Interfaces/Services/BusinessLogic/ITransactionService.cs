using IBot.Core.Interfaces.DTOs;

namespace IBot.Core.Interfaces.Services.BusinessLogic;

public interface ITransactionService
{
    public Task AppendTransaction(TransactionDto transaction);
}