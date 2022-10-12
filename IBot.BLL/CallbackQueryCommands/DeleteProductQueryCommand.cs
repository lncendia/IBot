using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.CallbackQueryCommands;

public class DeleteProductQueryCommand : ICallbackQueryCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, CallbackQuery query,
        ServiceContainer serviceContainer)
    {
        if (user!.State != State.Main)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Вы должны быть в главное меню.");
            return;
        }

        var id = Guid.Parse(query.Data![7..]);
        var product = await serviceContainer.UnitOfWork.ProductRepository.Value.GetAsync(id);
        if (product == null)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Такого продукта не существует.");
            return;
        }

        await serviceContainer.UnitOfWork.ProductRepository.Value.DeleteAsync(product);
        await serviceContainer.UnitOfWork.SaveAsync();

        await client.AnswerCallbackQueryAsync(query.Id, "Продукт успешно удалён.");
        await client.DeleteMessageAsync(query.Message!.Chat.Id, query.Message.MessageId);
    }

    public bool Compare(CallbackQuery query, User? user) => user!.IsAdmin && query.Data!.StartsWith("delete_");
}