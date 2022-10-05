using Telegram.Bot;
using Telegram.Bot.Types;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.Interfaces;

public interface ITextCommand
{
    public Task Execute(ITelegramBotClient client, User? user, Message message, ServiceContainer serviceContainer);

    public bool Compare(Message message, User? user);
}