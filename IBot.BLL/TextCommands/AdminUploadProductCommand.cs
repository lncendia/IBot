using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.TextCommands;

public class AdminUploadProductCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message, ServiceContainer serviceContainer)
    {
        if (user!.State is State.Main)
        {
            await client.SendTextMessageAsync(message.From!.Id,
                "Введите страницу и сообщение, которое хотите разослать.\nПример: <code>1:Привет, я бот!</code>\nОдна страница - 500 пользователей.",
                ParseMode.Html, replyMarkup: MainKeyboard.Main);
            user.State = State.MailingAdmin;
        }
        else
        {
            await client.SendTextMessageAsync(message.From!.Id, "Вы вышли из панели загрузки.",
                replyMarkup: MainKeyboard.MainReplyKeyboard);
            user.State = State.Main;
        }

        await serviceContainer.UnitOfWork.UserRepository.Value.UpdateAsync(user);
        await serviceContainer.UnitOfWork.SaveAsync();
    }

    public bool Compare(Message message, User? user) =>
        message.Type == MessageType.Text && message.Text!.StartsWith("/upload") &&
        user!.State is State.Main && user.IsAdmin;
}