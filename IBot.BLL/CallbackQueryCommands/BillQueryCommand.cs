using IBot.BLL.Interfaces;
using IBot.Core.Entities.Transactions;
using IBot.Core.Interfaces.Services.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.CallbackQueryCommands;

public class BillQueryCommand : ICallbackQueryCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, CallbackQuery query,
        ServiceContainer serviceContainer)
    {
        try
        {
            var payment = await serviceContainer.PaymentCreationService.GetPaymentAsync(query.Data![5..]);
            var transaction = new Transaction(payment.Amount, user!.Id, payment.PayTime);
            user.PerformTransaction(transaction);
            await serviceContainer.UnitOfWork.TransactionRepository.Value.AddAsync(transaction);
            await serviceContainer.UnitOfWork.UserRepository.Value.UpdateAsync(user);
            await serviceContainer.UnitOfWork.SaveAsync();

            var message = query.Message!.Text!;
            message = message.Replace("❌ Статус: Не оплачено", "✔ Статус: Оплачено");
            message = message.Remove(message.IndexOf("Оплачено", StringComparison.Ordinal) + 8);
            message += $"\n🗓 Дата оплаты: {payment.PayTime.ToLocalTime():g}";
            await client.EditMessageTextAsync(query.From.Id, query.Message.MessageId, message);
            await client.AnswerCallbackQueryAsync(query.Id, "Успешно оплачено.");
        }
        catch (BillCheckException)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Произошла ошибка при добавлении платежа.");
        }
        catch (BillNotPaidException)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Счёт не оплачен.");
        }
    }

    public bool Compare(CallbackQuery query, User? user) => query.Data!.StartsWith("bill_");
}