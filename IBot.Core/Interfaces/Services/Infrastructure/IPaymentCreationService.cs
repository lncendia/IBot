using IBot.Core.Interfaces.Services.DTOs;

namespace IBot.Core.Interfaces.Services.Infrastructure;

public interface IPaymentCreationService
{
    Task<PaymentDto> CreateBillAsync(Guid userId, decimal amount);
    Task<TransactionDto> GetPaymentAsync(string id);
}