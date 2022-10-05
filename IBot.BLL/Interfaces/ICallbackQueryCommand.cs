using Telegram.Bot;
using Telegram.Bot.Types;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.Interfaces;

public interface ICallbackQueryCommand
{
    public Task Execute(ITelegramBotClient client, User? user, CallbackQuery query, ServiceContainer serviceContainer);

    public bool Compare(CallbackQuery query, User? user);
}