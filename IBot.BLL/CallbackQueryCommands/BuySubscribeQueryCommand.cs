using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.CallbackQueryCommands;

public class BuySubscribeQueryCommand : ICallbackQueryCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, CallbackQuery query,
        ServiceContainer serviceContainer)
    {
        if (user!.State != State.Main)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Вы должны быть в главное меню.");
            return;
        }

        await client.EditMessageTextAsync(query.From.Id, query.Message!.MessageId,
            $"Введите количество аккаунтов, которые хотите добавить. Цена одного аккаунта - {serviceContainer.Configuration.Cost} рублей/30 дней.",
            replyMarkup: MainKeyboard.Main);
        user.State = State.EnterCountToBuy;
        await serviceContainer.UserService.UpdateAsync(user);
    }

    public bool Compare(CallbackQuery query, User? user)
    {
        return query.Data == "buySubscribe";
    }
}