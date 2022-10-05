using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.CallbackQueryCommands;

public class MainMenuQueryCommand : ICallbackQueryCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, CallbackQuery query,
        ServiceContainer serviceContainer)
    {
        user!.State = State.Main;
        await serviceContainer.UnitOfWork.UserRepository.Value.UpdateAsync(user);
        await serviceContainer.UnitOfWork.SaveAsync();
        await client.EditMessageTextAsync(query.From.Id, query.Message!.MessageId,
            "Вы в главном меню.");
    }

    public bool Compare(CallbackQuery query, User? user)
    {
        return query.Data == "mainMenu";
    }
}