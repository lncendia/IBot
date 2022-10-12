using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.CallbackQueryCommands;

public class TopUpAmountQueryCommand : ICallbackQueryCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, CallbackQuery query,
        ServiceContainer serviceContainer)
    {
        if (user!.State != State.Main)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Вы должны быть в главное меню.");
            return;
        }

        user.State = State.EnterAmount;
        await serviceContainer.UnitOfWork.UserRepository.Value.UpdateAsync(user);
        await serviceContainer.UnitOfWork.SaveAsync();
        await client.EditMessageTextAsync(query.From.Id, query.Message!.MessageId,
            $"Введите сумму, на которую хотите пополнить счёт. Минимальная сумма - 100 рублей.", replyMarkup: MainKeyboard.Main);
    }

    public bool Compare(CallbackQuery query, User? user) => query.Data == "buySubscribe";
}