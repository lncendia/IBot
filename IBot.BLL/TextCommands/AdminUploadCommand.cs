using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.TextCommands;

public class AdminUploadCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message, ServiceContainer serviceContainer)
    {
        if (user!.State is State.Main)
        {
            await client.SendTextMessageAsync(message.From!.Id, "Отправьте фото, которое будет отображаться в списке продуктов.", replyMarkup: MainKeyboard.Main);
            user.State = State.UploadProductPreviewAdmin;
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
        message.Type == MessageType.Text && message.Text! == "/upload" &&
        user!.State is State.Main && user.IsAdmin;
}