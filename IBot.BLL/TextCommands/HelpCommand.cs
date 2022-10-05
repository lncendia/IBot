using IBot.BLL.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.TextCommands;

public class HelpCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message,
        ServiceContainer serviceContainer)
    {
        await client.SendTextMessageAsync(message.Chat.Id,
            $"За поддержкой вы можете обратиться к {serviceContainer.Configuration.HelpUsername}.");
    }

    public bool Compare(Message message, User? user)
    {
        return message.Type == MessageType.Text && message.Text == "🤝 Поддержка";
    }
}