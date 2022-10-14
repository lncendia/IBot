using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using IBot.Core.Entities.Users.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.CallbackQueryCommands;

public class UploadProductQueryCommand : ICallbackQueryCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, CallbackQuery query,
        ServiceContainer serviceContainer)
    {
        if (user!.State != State.Main)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Вы должны быть в главное меню.");
            return;
        }

        await client.EditMessageTextAsync(query.From.Id, query.Message!.MessageId,
            "Отправьте фото, которое будет отображаться в списке продуктов.", replyMarkup: MainKeyboard.Main);
        user.State = State.UploadProductPreviewAdmin;
        await serviceContainer.UnitOfWork.UserRepository.Value.UpdateAsync(user);
        await serviceContainer.UnitOfWork.SaveAsync();
    }

    public bool Compare(CallbackQuery query, User? user) => query.Data!.StartsWith("uploadProduct") && user!.IsAdmin;
}