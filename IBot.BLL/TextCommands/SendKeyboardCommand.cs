using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.TextCommands;

public class SendKeyboardCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message,
        ServiceContainer serviceContainer)
    {
        user!.State = State.Main;
        await serviceContainer.UnitOfWork.UserRepository.Value.UpdateAsync(user);
        await serviceContainer.UnitOfWork.SaveAsync();
        await client.SendTextMessageAsync(message.From!.Id,
            "Вы в главном меню.", replyMarkup: MainKeyboard.MainReplyKeyboard);
    }

    public bool Compare(Message message, User? user)
    {
        return message.Type == MessageType.Text && message.Text!.StartsWith("/start");
    }
}