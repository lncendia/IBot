using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.TextCommands;

public class UploadPreviewCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message, ServiceContainer serviceContainer)
    {
        
        user!.TempProductPreview = message.Photo!.LastOrDefault()?.FileId;
        user.State = State.UploadProductAdmin;
        await serviceContainer.UnitOfWork.UserRepository.Value.UpdateAsync(user);
        await serviceContainer.UnitOfWork.SaveAsync();
        await client.SendTextMessageAsync(message.From!.Id, "Фото загружено. Отправьте продукт.", replyMarkup: MainKeyboard.Main);
    }

    public bool Compare(Message message, User? user) =>
        message.Type == MessageType.Photo && user!.State == State.UploadProductPreviewAdmin && user.IsAdmin;
}