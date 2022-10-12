using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.CallbackQueryCommands;

public class MyTransactionsQueryCommand : ICallbackQueryCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, CallbackQuery query,
        ServiceContainer serviceContainer)
    {
        if (user!.State != State.Main)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Вы должны быть в главное меню.");
            return;
        }

        var page = int.Parse(query.Data![16..]);
        if (page < 1)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Вы в конце списка.");
            return;
        }

        var transaction =
            await serviceContainer.UnitOfWork.TransactionRepository.Value.GetAsync(user.Transactions[page - 1]);
        if (transaction == null)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Транзакция не найдена.");
            return;
        }

        await client.EditMessageTextAsync(query.From.Id, query.Message!.MessageId,
            $"<b>Сумма:</b> <code>{transaction.Cost}</code> руб.\n<b>Дата:</b> <code>{transaction.Date.ToLocalTime():g}</code>",
            ParseMode.Html,
            replyMarkup: PaymentKeyboard.MyTransactions(page, page <= user.Transactions.Count));
    }

    public bool Compare(CallbackQuery query, User? user) => query.Data!.StartsWith("myTransactions_");
}