using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using IBot.Core.Entities.Users.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.CallbackQueryCommands;

public class GetProductQueryCommand : ICallbackQueryCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, CallbackQuery query,
        ServiceContainer serviceContainer)
    {
        if (user!.State != State.Main)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Вы должны быть в главное меню.");
            return;
        }

        var id = Guid.Parse(query.Data![4..]);

        var product = await serviceContainer.UnitOfWork.ProductRepository.Value.GetAsync(id);
        if (product == null)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Предложение не найдено.");
            return;
        }

        if (!user.Products.Contains(id))
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Предложение не куплено.");
            return;
        }

        await client.SendDocumentAsync(query.From.Id, new InputOnlineFile(product.DataId), caption: "Успешно.");
    }

    public bool Compare(CallbackQuery query, User? user) => query.Data!.StartsWith("get_");
}