using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.TextCommands;

public class UploadProductCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message, ServiceContainer serviceContainer)
    {
        user!.TempProduct = message.Document!.FileId;
        user.State = State.EnterNameAndAmountAdmin;
        await serviceContainer.UnitOfWork.UserRepository.Value.UpdateAsync(user);
        await serviceContainer.UnitOfWork.SaveAsync();
        await client.SendTextMessageAsync(message.From!.Id,
            "Продукт загружен. Введите его название и стоимость.\n\nФормат: <code>99,9:Крутое название[до 20 символов]</code>",
            ParseMode.Html, replyMarkup: MainKeyboard.Main);
    }

    public bool Compare(Message message, User? user) =>
        message.Type == MessageType.Document && user!.State == State.UploadProductAdmin && user.IsAdmin;
}