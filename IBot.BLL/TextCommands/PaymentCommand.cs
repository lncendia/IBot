using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.TextCommands;

public class PaymentCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message,
        ServiceContainer serviceContainer)
    {
        await client.SendTextMessageAsync(message.Chat.Id,
            "Выберите, что вы хотите сделать.",
            replyMarkup: PaymentKeyboard.Subscribes);
    }

    public bool Compare(Message message, User? user)
    {
        return message.Type == MessageType.Text && message.Text == "💸 Баланс" && user!.State == State.Main;
    }
}