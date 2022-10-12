using IBot.BLL.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.TextCommands;

public class BanCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message, ServiceContainer serviceContainer)
    {
        await client.SendTextMessageAsync(message.Chat.Id,
            $"Вы были заблокированы. Для обжалования обратитесь в поддержку: {serviceContainer.Configuration.HelpUsername}.");
    }

    public bool Compare(Message message, User? user)
    {
        return user!.IsBanned;
    }
}