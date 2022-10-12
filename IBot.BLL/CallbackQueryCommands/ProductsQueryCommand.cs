using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.CallbackQueryCommands;

public class ProductsQueryCommand : ICallbackQueryCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, CallbackQuery query,
        ServiceContainer serviceContainer)
    {
        if (user!.State != State.Main)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Вы должны быть в главное меню.");
            return;
        }

        var page = int.Parse(query.Data![9..]);
        if (page < 1)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Вы в конце списка.");
            return;
        }

        const int perPage = 10;

        var products =
            await serviceContainer.UnitOfWork.ProductRepository.Value.Find((page - 1) * perPage, perPage);
        if (!products.Any())
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Больше нет предложений.");
            return;
        }

        await client.EditMessageTextAsync(query.From.Id, query.Message!.MessageId, "Выберите товар:",
            replyMarkup: ProductKeyboard.Create(products, page, products.Count == perPage));
    }

    public bool Compare(CallbackQuery query, User? user) => query.Data!.StartsWith("products_");
}