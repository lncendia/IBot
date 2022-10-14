using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.CallbackQueryCommands;

public class MyProductsQueryCommand : ICallbackQueryCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, CallbackQuery query,
        ServiceContainer serviceContainer)
    {
        if (user!.State != State.Main)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Вы должны быть в главное меню.");
            return;
        }

        var page = int.Parse(query.Data![11..]);
        if (page < 1)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Вы в конце списка.");
            return;
        }

        if (user.Products.Count < page)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Неверный номер страницы.");
            return;
        }

        var product = await serviceContainer.UnitOfWork.ProductRepository.Value.GetAsync(user.Products[page - 1]);
        var keyboard = ProductKeyboard.MyProducts(product, page, page < user.Products.Count);
        if (product == null)
            await client.SendTextMessageAsync(query.From.Id, "Предложение не найдено.", ParseMode.Html,
                replyMarkup: keyboard);
        else if (query.Message!.Type != MessageType.Photo)
            await client.SendPhotoAsync(query.From.Id, new InputOnlineFile(product.PreviewId),
                $"<b>Название:</b> <code>{product.Name}</code>", ParseMode.Html, replyMarkup: keyboard);
        else
        {
            await client.EditMessageMediaAsync(query.From.Id, query.Message.MessageId,
                new InputMediaPhoto(new InputMedia(product.PreviewId)));
            await client.EditMessageCaptionAsync(query.From.Id, query.Message.MessageId,
                $"<b>Название:</b> <code>{product.Name}</code>", ParseMode.Html, replyMarkup: keyboard);
        }
    }

    public bool Compare(CallbackQuery query, User? user) => query.Data!.StartsWith("myProducts_");
}